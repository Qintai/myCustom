using log4net.Repository;
using Microsoft.AspNetCore.Hosting;
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
            InItLog4 initlog4 = InItLog4.Instance;

            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        }
    }
}
