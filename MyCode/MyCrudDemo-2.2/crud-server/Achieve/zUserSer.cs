using System.Linq;
using Chloe;
using crud_base;
using crud_entity;
using crud_server.connector;

namespace crud_server.Achieve
{
    public class zUserSer : BaseMysql<zUser>, IzUserSer
    {
        public zUserSer(IDbContext _context)
            : base(_context)
        {
        }

        public override zUser GetModel(int Id)
        {
            return context.Query<zUser>().First(a => a.Id == Id);
        }

        /// <summary>
        ///     是否有年龄等于18的用户
        /// </summary>
        /// <returns></returns>
        public int AgeGreater18()
        {
            var age = 18;
            var res = context.SqlQuery<int>("select count(1) from Users where Age=?age", new DbParam("?age", age))
                .Count();
            //  int res2 = context.SqlQuery<int>("select count(1) from Users where Age=?age", new DbParam("?age", age)).Count();
            //int users = context.FormatSqlQuery<int>($"select * from Users where age={age}").Count(); //扩展方法，需要下载Chioe的扩展
            return res;
        }
    }
}