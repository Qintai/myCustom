using log4net;

namespace crud_base
{
    public class Log4help
    {
        private static readonly ILog log;

        static Log4help()
        {
            log = LogManager.GetLogger(typeof(Log4help)); //这一种无需指定 日志仓库
        }

        public static void Info(dynamic msg)
        {
            log.Info(msg);
        }

        public static void Errlog(dynamic msg)
        {
            log.Error(msg);
        }
    }
}