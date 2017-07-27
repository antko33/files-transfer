using System;
using System.IO;
using System.ServiceModel;

namespace FileServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "FileServer" в коде и файле конфигурации.
    [ServiceBehavior]
    public class FileServer : IFileServer
    {
        const string SERVER_DIR = "FILES";  //Папка, в которую помещаются загруженные файлы
        const int BUFFER_SIZE = 1024;   //Размер буфера

        /* Удаление файла по имени */
        public void FileDelete(string filename)
        {
            File.Delete(Path.Combine(SERVER_DIR, filename));
        }
        
        /* Загрузка файла на сервер */
        public void ProvideInfo(Stream data)
        {
            /* Первоначально файлу присваивается временное имя - unix-метка времени его создания */
            string unixTime = ((long)(DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
            string tmp_name = Path.Combine(SERVER_DIR, unixTime);

            byte[] bytes = new byte[BUFFER_SIZE];
            FileStream file_w = File.Open(tmp_name, FileMode.Create);

            // Читаем поток
            while (data.Read(bytes, 0, BUFFER_SIZE) > 0)
            {
                file_w.Write(bytes, 0, BUFFER_SIZE);
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
            string tmp_name = "0";

            // Поиск последнего файла с временным именем
            foreach (string file in Directory.GetFiles(SERVER_DIR))
            {
                string[] safe = file.Split('\\');
                string sfile = safe[safe.Length - 1];
                try
                {
                    if (long.Parse(sfile) > long.Parse(tmp_name)) tmp_name = sfile;
                }
                catch (Exception) { }
            }
            File.Move(Path.Combine(SERVER_DIR, tmp_name), Path.Combine(SERVER_DIR, name));
        }
    }
}