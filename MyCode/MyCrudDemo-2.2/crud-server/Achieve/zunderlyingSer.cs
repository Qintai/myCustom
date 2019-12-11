using Chloe;
using crud_base;
using crud_entity;


namespace crud_server.connector
{
    public class IzunderlyingSer : BaseMysql<zunderlying>
    {
        public IzunderlyingSer(IDbContext _context)
            : base(_context)
        {
        
        }
     
        public override zunderlying GetModel(int Id)
        {
            return null;
        }



    }
}
