using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QinCommon.Common
{
    /// <summary>
    /// json文件 操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }

        private string _filePath;

        public Appsettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
               .Build();
        }

        public static string GetConfig(string fileName, string contentPath)
        {

            


            if (!string.IsNullOrWhiteSpace(fileName))
            {
                //var ae=AppContext.TargetFrameworkName;
                //var baseDir = AppContext.BaseDirectory;
                //var indexSrc = baseDir.IndexOf("src");
                //var subToSrc = baseDir.Substring(0, indexSrc);
                //var currentClassDir = subToSrc + "src" + Path.DirectorySeparatorChar + "QinOpen";

                Configuration = new ConfigurationBuilder()
                   .SetBasePath(@"E:\LC\myCustom\open\QinOpen\QinOpen\bin\Debug\netcoreapp3.0\IApplicationBuilderExtend")
                   .Add(new JsonConfigurationSource { Path = fileName, Optional = false, ReloadOnChange = true })
                   .Build();

                return Configuration[contentPath];
            }
            return "";
        }




        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {

                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { }

            return "";
        }
    }
}
