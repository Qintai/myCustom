using Autofac.Extensions.DependencyInjection;
using log4net.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QinCommon;
using QinEntity;
using SqlSugar;
using System;
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

            //�����еĽ���
            {
                // dotnet QinOpen.dll--name = pp--age = 99

                var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                        .AddCommandLine(args); //���Խ��յ���Ӧ�ó������

                var configuration = builder.Build();
                System.Console.WriteLine($"name��{configuration["name"]}");
                System.Console.WriteLine($"age��{configuration["age"]}");
            }

            IHost host = CreateHostBuilder(args).Build();

            //=======sqllite�������ݿ�,��ʼ������=======//
            //var sclient = host.Services.GetRequiredService<SqlSugar.ISqlSugarClient>();
            //DbSeed dbSeed = host.Services.GetRequiredService<DbSeed>();
            //dbSeed.InitData(sclient);
            //===============//
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)

               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://localhost:8022");
                    webBuilder.UseStartup<Startup>();
                });

        }
    }
}
