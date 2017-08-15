using System.ServiceModel;
using System.IO;
using System.Collections.Generic;

namespace FileServer
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IFileServer" в коде и файле конфигурации.
    [ServiceContract]
    public interface IFileServer
    {
        [OperationContract]
        void ProvideInfo(Stream data); //Получение

        [OperationContract]
        Stream RequestInfo(string name);  //Отправка

        [OperationContract]
        void FileDelete(string filename);

        [OperationContract]
        string[] GetFiles();

        [OperationContract]
        void RenameUploadedFile(string name);

        [OperationContract]
        List<string> SplitFile(string name);

        [OperationContract]
        void MergeFiles(string[] names, string result_name);
    }
}
