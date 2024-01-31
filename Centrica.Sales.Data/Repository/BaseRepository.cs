using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Repository
{
    public class BaseRepository
    {
        protected readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
