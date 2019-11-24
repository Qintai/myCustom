using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace QinCommon
{
    /// <summary>
    /// 初始化Log4
    /// </summary>
    public class InItLog4
    {
        private static readonly InItLog4 _instance = null;
        static InItLog4()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("Log4net.config"));
            ILoggerRepository repo = log4net.LogManager.CreateRepository
                (Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            _instance = new InItLog4();
        }
        public static InItLog4 Instance
        {
            get { return _instance; }
        }
        private InItLog4() { }
    }

    /// <summary>
    /// 日常写日志
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Log4helper<T>
    {
        static log4net.ILog log;

        static Dictionary<string, Type> keyValues = new Dictionary<string, Type>();

        /// <summary>
        /// 静态缓存。保存这个类型第一次被反射的结果，只会执行一次。除非T类型有更换
        /// </summary>
        static Log4helper()
        {
            Type t = typeof(T);
            log = LogManager.GetLogger(t/*typeof(GlobalExceptionsFilter)*/);
            keyValues.Add(t.Name, t);
        }

        public static void Info(string str)
        {
            log.Info(str);
        }
        public static void Errror(Exception e)
        {
            log.Error(e);
        }

        public static void Debug(string s)
        {
            log.Debug(s);
        }
    }
}