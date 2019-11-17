using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace open.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder =>
                {
                  // loggingBuilder.AddFilter("System", LogLevel.Warning);
                  // loggingBuilder.AddFilter("Microsoft", LogLevel.Warning); //过滤掉系统默认的一些日志
                  // loggingBuilder.AddLog4Net(@"Log4Config\log4net.Config"); //配置文件地址
                })
                .UseStartup<Startup>();
        }


        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();
    }
}
