using System.Data;
using Chloe.Infrastructure;
using MySql.Data.MySqlClient;

namespace crud_base
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connString;

        public DbConnectionFactory(string conn)
        {
            _connString = conn;
        }

        public IDbConnection CreateConnection()
        {
            IDbConnection conn = new MySqlConnection(_connString);
            return conn;
        }
    }
}