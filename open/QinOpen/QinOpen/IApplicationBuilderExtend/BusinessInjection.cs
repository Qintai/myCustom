using Chloe;
using Chloe.Infrastructure;
using Chloe.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

namespace QinOpen
{
    /// <summary>
    ///     配置数据库 的链接
    /// </summary>
    public static class DBInit
    {
        public static void DbInitialization(this IServiceCollection services, IConfiguration configuration)
        {
            #region SqlSuger
            //services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            //{
            //    return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            //    {
            //        ConnectionString = BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
            //        DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
            //        IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
            //        InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
            //    });
            //});
            #endregion


            #region ChIoe
            services.AddScoped<IDbContext>(serviceProvider =>
            {
                string _connString = configuration.GetSection("con:consetting").Value;
                return new MySqlContext(new DbConnectionFactory(() =>
                {
                    IDbConnection conn = new MySqlConnection(_connString);
                    return conn;
                }));
            });
            #endregion


        }


    }
}