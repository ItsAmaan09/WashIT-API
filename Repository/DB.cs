using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace washit.repository
{
    public class DB
    {
        private readonly IConfiguration _config;

        public DB(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("WashitDB"));
        }
    }
}