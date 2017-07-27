using System;
using System.ServiceModel;
using System.IO;

namespace FileServerHost
{
    class Program
    {
        const string DIR = "FILES";
        static void Main()
        {
            using (var host = new ServiceHost(typeof(FileServer.FileServer)))
            {
                

                if (!Directory.Exists(DIR)) Directory.CreateDirectory(DIR);
                host.Open();
                Console.WriteLine("host opened");
                Console.ReadLine();
            }
        }
    }
}
