using System.ServiceModel;
using System.IO;

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
    }
}
