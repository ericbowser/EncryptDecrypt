// See https://aka.ms/new-console-template for more information
using EncryptDecrypt;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .Build();
        
        
        //int stop = -1;
        //while (stop == -1)
        //{
        var getThing = Utils.EncryptPass();

        Console.WriteLine(getThing);
        //}

        Console.ReadKey();
    }
}
