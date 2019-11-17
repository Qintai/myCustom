using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;

namespace QinCommon.Common
{
    /// <summary>
    /// json文件 操作类
    /// </summary>
    public class Appsettings
    {
        static IConfiguration Configuration { get; set; }

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

        /// <summary>
        /// 获取指定目录下json文件的内容
        /// </summary>
        /// <param name="basepath">json文件所在的父级目录</param>
        /// <param name="fileName">json文件名称</param>
        /// <param name="contentPath">json 节点</param>
        /// <returns></returns>
        public static string GetFilePathConfig(string basepath,string fileName, string contentPath)
        {
            basepath = @"E:\LC\myCustom\open\QinOpen\QinOpen\bin\Debug\netcoreapp3.0\IApplicationBuilderExtend";
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                Configuration = new ConfigurationBuilder()
                   .SetBasePath(basepath)
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
                    return Configuration[string.Join(":", sections)];
            }
            catch (Exception) { }
            return "";
        }
    }
}
