using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QinCommon.Common.DB;

namespace QinOpen
{
    public static class DBInit
    {
        /// <summary>
        ///  配置数据库的链接
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void DbInitialization(this IServiceCollection services, IConfiguration configuration)
        {
            #region SqlSuger，注入单例 ISqlSugarClient
            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    // "Server=localhost;Port=3306;Database=ut;Uid=root;Pwd=root;CharSet=utf8;CharSet=utf8;"
                    ConnectionString = BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                    DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                }); ;
            });
            #endregion

        }
    }
}