﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileClient.FileServer {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="FileServer.IFileServer")]
    public interface IFileServer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/GetSum", ReplyAction="http://tempuri.org/IFileServer/GetSumResponse")]
        int GetSum(int x, int y);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/GetSum", ReplyAction="http://tempuri.org/IFileServer/GetSumResponse")]
        System.Threading.Tasks.Task<int> GetSumAsync(int x, int y);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/ProvideInfo", ReplyAction="http://tempuri.org/IFileServer/ProvideInfoResponse")]
        void ProvideInfo(System.IO.Stream data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/ProvideInfo", ReplyAction="http://tempuri.org/IFileServer/ProvideInfoResponse")]
        System.Threading.Tasks.Task ProvideInfoAsync(System.IO.Stream data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/RequestInfo", ReplyAction="http://tempuri.org/IFileServer/RequestInfoResponse")]
        System.IO.Stream RequestInfo(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/RequestInfo", ReplyAction="http://tempuri.org/IFileServer/RequestInfoResponse")]
        System.Threading.Tasks.Task<System.IO.Stream> RequestInfoAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/FileDelete", ReplyAction="http://tempuri.org/IFileServer/FileDeleteResponse")]
        void FileDelete(string filename);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/FileDelete", ReplyAction="http://tempuri.org/IFileServer/FileDeleteResponse")]
        System.Threading.Tasks.Task FileDeleteAsync(string filename);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/GetFiles", ReplyAction="http://tempuri.org/IFileServer/GetFilesResponse")]
        string[] GetFiles();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/GetFiles", ReplyAction="http://tempuri.org/IFileServer/GetFilesResponse")]
        System.Threading.Tasks.Task<string[]> GetFilesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/RenameUploadedFile", ReplyAction="http://tempuri.org/IFileServer/RenameUploadedFileResponse")]
        void RenameUploadedFile(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileServer/RenameUploadedFile", ReplyAction="http://tempuri.org/IFileServer/RenameUploadedFileResponse")]
        System.Threading.Tasks.Task RenameUploadedFileAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileServerChannel : FileClient.FileServer.IFileServer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileServerClient : System.ServiceModel.ClientBase<FileClient.FileServer.IFileServer>, FileClient.FileServer.IFileServer {
        
        public FileServerClient() {
        }
        
        public FileServerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileServerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileServerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int GetSum(int x, int y) {
            return base.Channel.GetSum(x, y);
        }
        
        public System.Threading.Tasks.Task<int> GetSumAsync(int x, int y) {
            return base.Channel.GetSumAsync(x, y);
        }
        
        public void ProvideInfo(System.IO.Stream data) {
            base.Channel.ProvideInfo(data);
        }
        
        public System.Threading.Tasks.Task ProvideInfoAsync(System.IO.Stream data) {
            return base.Channel.ProvideInfoAsync(data);
        }
        
        public System.IO.Stream RequestInfo(string name) {
            return base.Channel.RequestInfo(name);
        }
        
        public System.Threading.Tasks.Task<System.IO.Stream> RequestInfoAsync(string name) {
            return base.Channel.RequestInfoAsync(name);
        }
        
        public void FileDelete(string filename) {
            base.Channel.FileDelete(filename);
        }
        
        public System.Threading.Tasks.Task FileDeleteAsync(string filename) {
            return base.Channel.FileDeleteAsync(filename);
        }
        
        public string[] GetFiles() {
            return base.Channel.GetFiles();
        }
        
        public System.Threading.Tasks.Task<string[]> GetFilesAsync() {
            return base.Channel.GetFilesAsync();
        }
        
        public void RenameUploadedFile(string name) {
            base.Channel.RenameUploadedFile(name);
        }
        
        public System.Threading.Tasks.Task RenameUploadedFileAsync(string name) {
            return base.Channel.RenameUploadedFileAsync(name);
        }
    }
}
