using Autofac.Extensions.DependencyInjection;
using log4net.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using QinCommon;
using System.IO;
using System.Reflection;
using System.Xml;

namespace QinOpen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InItLog4.InitLog4net();

            //命令行的接受
            {
                // dotnet QinOpen.dll--name = pp--age = 99

                var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                        .AddCommandLine(args);  //可以接收调试应用程序参数
                var configuration = builder.Build();
                System.Console.WriteLine($"name：{configuration["name"]}");
                System.Console.WriteLine($"age：{configuration["age"]}");
            }



            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)

               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        }
    }
}
