using Chloe;
using crud_base;
using crud_entity;

namespace crud_server.Achieve
{
    public class zmyhorkSer : BaseMysql<zmyhork>
    {
        public zmyhorkSer(IDbContext _context)
            : base(_context)
        {
        }

        public override zmyhork GetModel(int Id)
        {
            return context.Query<zmyhork>().First(a => a.Id == Id);
        }
    }
}