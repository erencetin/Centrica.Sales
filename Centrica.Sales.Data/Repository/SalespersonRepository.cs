using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Centrica.Sales.Data.Interfaces;
using System.Data.Common;

namespace Centrica.Sales.Data.Repository
{
    public class SalespersonRepository : BaseRepository, ISalespersonRepository
    {
        public SalespersonRepository(string connectionString) : base(connectionString) { }

        public async Task<Salesperson> GetByIdAsync(Guid id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, FirstName, LastName FROM Salesperson WHERE Id = @Id";
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });
                var dbCommand = (DbCommand)command;
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Salesperson
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
