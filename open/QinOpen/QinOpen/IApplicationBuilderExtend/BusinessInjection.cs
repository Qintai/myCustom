using Chloe;
using Chloe.Infrastructure;
using Chloe.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using QinCommon.Common;
using QinCommon.Common.DB;
using QinServices;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace QinOpen
{
    /// <summary>
    ///   
    /// </summary>
    public static class DBInit
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void DbInitialization(this IServiceCollection services, IConfiguration configuration)
        {
            #region SqlSuger
            services.AddScoped<SqlSugar.ISqlSugarClient>(o =>
            {
                return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
                {
                    // "Server=localhost;Port=3306;Database=ut;Uid=root;Pwd=root;CharSet=utf8;CharSet=utf8;"
                    ConnectionString =BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                    DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                    IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
                }); ;
            });

            services.AddTransient<zCustomUserService>();

            #endregion


            #region ChIoe
           /* services.AddScoped<IDbContext>(serviceProvider =>
            {
               string _connString =   Appsettings.app(new string[] { "AppSettings", "MySql", "MySqlConnection" });
                return new MySqlContext(new DbConnectionFactory(() =>
                {
                    IDbConnection conn = new MySqlConnection(_connString);
                    return conn;
                }));
            });
            //先注入 IDbContext，再注入业务
            Assembly assembly = Assembly.Load("QinServices");
            Type[] types = assembly.GetTypes();
            foreach (Type item in types)
            {
                var inters = item.GetInterfaces().Where(k => !k.Name.Contains("IBaseRepository") && !k.IsClass).ToArray();
                if (item.IsClass && (inters == null || inters.Count() == 0))
                    services.AddTransient(item); // 注册单独类
                else
                    foreach (var inter in inters)
                        services.AddTransient(inter, item); //注册 接口--实现类
            }*/
            #endregion

        }
    }
}