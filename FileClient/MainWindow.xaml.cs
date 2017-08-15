using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FileClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int BUFFER_SIZE = 1024;   // Размер буфера
        const int PART_SIZE = 1048576; // Размер "куска"
        const string SERVER_DIR = "FILES";  //Папка, в которую помещаются загруженные файлы
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var client = new FileServer.FileServerClient("BasicHttpBinding_IFileServer");
            var name = new OpenFileDialog();    // Выбор файла для загрузки на сервер
            name.Multiselect = false;
            if (name.ShowDialog() ?? false)
            {
                List<string> parts = new List<string>();
                if (name.FileName == "") return;
                try
                {
                    parts = SplitFile(name.FileName);   // Разбиение файла на части
                    var filesBefore = client.GetFiles();    // Список файлов до операции
                    foreach (string part in parts)
                    {
                        // Если часть файла уже загружена, пропускаем её
                        if (filesBefore.Contains(Path.Combine(SERVER_DIR, part.Split('\\').Last()))) continue; 
                        FileStream file = new FileStream(part, FileMode.Open);
                        client.ProvideInfo(file);
                        client.RenameUploadedFile(file.Name.Split('\\').Last());   // Переименование файла с временным именем
                        file.Close();
                    }

                    var filesAfter = client.GetFiles(); // Список фалов после операции
                    IEnumerable<string> uploadedFiles = filesAfter.Except(filesBefore); // Список загруженных файлов
                    client.MergeFiles(uploadedFiles.ToArray(), Path.Combine(SERVER_DIR, name.SafeFileName));
                    foreach (string file in uploadedFiles)
                    {
                        client.FileDelete(file.Split('\\').Last());
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                    return;
                }
                finally
                {
                    foreach (string part in parts)
                    {
                        if (File.Exists(part)) File.Delete(part);  //  Удаление временных файлов
                    }
                }
            }
            else return;
            client.Close();
            listBox.Items.Add(name.SafeFileName);   // Добавление имени файла в список
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            var client = new FileServer.FileServerClient("BasicHttpBinding_IFileServer");
            string filename = listBox.SelectedItem.ToString();  //  Выбираем файл для загрузки
            if (listBox.SelectedItem == null) return;

            var saveFD = new SaveFileDialog();  // Диалоговое окно сохранения файла
            saveFD.CheckFileExists = false; //  Чтобы не ругался на несуществующее имя файла
            if (saveFD.ShowDialog() ?? false)
            {
                List<string> parts = new List<string>();
                try
                {
                    parts = new List<string>(client.SplitFile(Path.Combine(SERVER_DIR, filename)));   // Разбиваем файл на части
                    string[] arrDirName = saveFD.FileName.Split('\\');
                    Array.Resize<string>(ref arrDirName, arrDirName.Length - 1);
                    string dirName = string.Join<string>("\\", arrDirName); // Имя папки для загрузки

                    var filesBefore = Directory.GetFiles(dirName);

                    foreach (string part in parts)
                    {
                        if (filesBefore.Contains(Path.Combine(dirName, part.Split('\\').Last()))) continue;

                        Stream file = client.RequestInfo(part);
                        byte[] bytes = new byte[BUFFER_SIZE];
                        FileStream file_w = new FileStream(Path.Combine(dirName, part.Split('\\').Last()), FileMode.Create);

                        int size;
                        while ((size = file.Read(bytes, 0, BUFFER_SIZE)) > 0)    //  Чтение из потока
                        {
                            //foreach (byte one in bytes)
                            //{
                            //    file_w.WriteByte(one);
                            //}
                            file_w.Write(bytes, 0, size);
                        }
                        file_w.Close();
                        file.Close();
                    }

                    var filesAfter = Directory.GetFiles(dirName);
                    IEnumerable<string> downloadedFiles = filesAfter.Except(filesBefore);

                    MergeFiles(parts.ToArray(), saveFD.FileName);

                    foreach (string file in downloadedFiles)
                    {
                        if (File.Exists(file)) File.Delete(file);   // Удаление временных файлов
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                    return;
                }
                finally
                {
                    foreach (string part in parts)
                    {
                        client.FileDelete(part);
                    }
                }
            }
        }

        /* Команды, выполняемые при зарузке главного окна */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var client = new FileServer.FileServerClient("BasicHttpBinding_IFileServer");

            /* Получение имён загруенных файлов */
            listBox.Items.Clear();
            try
            {
                foreach (string name in client.GetFiles())
                {
                    //  Оставляем только имя
                    string[] safe = name.Split('\\');
                    string sname = safe[safe.Length - 1];
                    listBox.Items.Add(sname);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }

            client.Close();

            listBox.SelectionMode = SelectionMode.Single; //    Можно выбрать только 1 элемент за 1 раз
        }

        /* Удаляет указанный файл */
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var client = new FileServer.FileServerClient("BasicHttpBinding_IFileServer");
            try
            {
                string item = listBox.SelectedItem.ToString();  //  Имя удаляемого файла

                listBox.Items.Remove(item);

            
                client.FileDelete(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }

            client.Close();
        }

        /* Разбивает файл с указанным именем на части размером PART_SIZE байт
         * Возвращает список имён полученных файлов */
        private List<string> SplitFile(string name)
        {
            List<string> files = new List<string>();
            using (FileStream from = new FileStream(name, FileMode.Open))
            {
                long parts = from.Length / PART_SIZE + 1;   // Количество частей, на которые нуно разбить файл
                for (long i = 1; i <= parts; i++)
                {
                    using (FileStream to = new FileStream(name + ".part" + i + "_" + parts, FileMode.OpenOrCreate))
                    {
                        long counter = PART_SIZE;   // Счётчик, определяющий количество записанных в файл байт
                        while(from.Position < from.Length && counter > 0)
                        {
                            to.WriteByte((byte)from.ReadByte());
                            counter--;
                        }
                        files.Add(to.Name);
                    }
                }
            }
            return files;
        }

        /* Объединяет указанные файлы в один */
        private void MergeFiles(string[] names, string result_name)
        {
            using (FileStream to = new FileStream(result_name, FileMode.OpenOrCreate))
            {
                foreach (string file in names)
                {
                    using (FileStream from = new FileStream(file, FileMode.Open))
                    {
                        while (from.Position < from.Length)
                            to.WriteByte((byte)from.ReadByte());
                    }
                }
            }
        }
    }
}
