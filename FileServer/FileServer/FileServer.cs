using System;
using System.IO;
using System.ServiceModel;
using System.Collections.Generic;

namespace FileServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "FileServer" в коде и файле конфигурации.
    [ServiceBehavior]
    public class FileServer : IFileServer
    {
        const string SERVER_DIR = "FILES";  //Папка, в которую помещаются загруженные файлы
        const int BUFFER_SIZE = 1024;   //Размер буфера
        const int PART_SIZE = 1048576;

        /* Удаление файла по имени */
        public void FileDelete(string filename)
        {
            File.Delete(Path.Combine(SERVER_DIR, filename));
        }
        
        /* Загрузка файла на сервер */
        public void ProvideInfo(Stream data)
        {
            /* Первоначально файлу присваивается временное имя - guid */
            string unixTime = ((long)(DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
            string tmp_name = Path.Combine(SERVER_DIR, Guid.NewGuid().ToString());//Path.Combine(SERVER_DIR, unixTime);

            byte[] bytes = new byte[BUFFER_SIZE];
            FileStream file_w = File.Open(tmp_name, FileMode.Create);

            // Читаем поток
            int size;
            while ((size = data.Read(bytes, 0, BUFFER_SIZE)) > 0)
            {
                file_w.Write(bytes, 0, size);
            }
            file_w.Close();
        }

        /* Скачивание файла с сервера */
        public Stream RequestInfo(string name)
        {
            FileStream file = new FileStream(Path.Combine(SERVER_DIR, name), FileMode.Open);
            return file;
        }

        /* Получение списка файлов (выполняется при открытии клиентского приложения */
        public string[] GetFiles()
        {
            return Directory.GetFiles(SERVER_DIR);
        }

        /* Переименование файла с временным именем */
        public void RenameUploadedFile(string name)
        {
            // Поиск последнего файла с временным именем
            DateTime lastTime = new DateTime(1970, 1, 1);
            string lastFile = "";
            foreach (string file in Directory.GetFiles(SERVER_DIR))
            {
                if (File.GetCreationTime(file) > lastTime)
                {
                    lastFile = file;
                    lastTime = File.GetCreationTime(file);
                }
            }

            File.Move(lastFile, Path.Combine(SERVER_DIR, name));
        }

        public List<string> SplitFile(string name)
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
                        while (from.Position < from.Length && counter > 0)
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
        public void MergeFiles(string[] names, string result_name)
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