using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System;

namespace FileClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int BUFFER_SIZE = 1024;
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
                if (name.FileName == "") return;
                try
                {
                    FileStream file = new FileStream(name.FileName, FileMode.Open);
                    client.ProvideInfo(file);
                    file.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                    return;
                }
            }
            else return;

            try
            {
                client.RenameUploadedFile(name.SafeFileName);   // Переименование файла с временным именем
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return;
            }

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
                try
                {
                    Stream file = client.RequestInfo(filename);
                    byte[] bytes = new byte[BUFFER_SIZE];
                    FileStream file_w = new FileStream(saveFD.FileName, FileMode.Create);

                    while (file.Read(bytes, 0, BUFFER_SIZE) > 0)    //  Чтение из потока
                    {
                        file_w.Write(bytes, 0, BUFFER_SIZE);
                    }
                    file_w.Close();
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                    return;
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
    }
}
