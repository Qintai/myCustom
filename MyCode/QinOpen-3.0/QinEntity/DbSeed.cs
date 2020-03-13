using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinEntity
{
    public class DbSeed
    {
        public void InitData(ISqlSugarClient sclient)
        {
            sclient.DbMaintenance.CreateDatabase(); //创建数据库
            sclient.CodeFirst.InitTables(typeof(zCustomUser));

            SimpleClient simpleClient = new SimpleClient(sclient);
            List<zCustomUser> userlist = new List<zCustomUser>()
            {
                   new zCustomUser(){  Name="admin",AddTime=DateTime.Now,Gender=1,pwd="",state=1},
                   new zCustomUser(){  Name="bob",AddTime=DateTime.Now,Gender=1,pwd="",state=1}
            };
            simpleClient.InsertRange(userlist);

        }
    }
}
