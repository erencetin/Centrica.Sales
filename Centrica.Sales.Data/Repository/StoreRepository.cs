using Centrica.Sales.Data.Interfaces;
using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Centrica.Sales.Data.Repository
{
    public class StoreRepository : BaseRepository, IStoreRepository
    {
        public StoreRepository(string connectionString) : base(connectionString) { }

        public async Task<Store> GetByIdAsync(Guid id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, [Name], DistrictId FROM Store WHERE Id = @Id";
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });
                var dbCommand = (DbCommand)command;
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Store
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            DistrictId = reader.GetGuid(reader.GetOrdinal("DistrictId"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public async Task<List<Store>> GetStoresByDistrictIdAsync(Guid districtId)
        {
            var stores = new List<Store>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Id, [Name],DistrictId FROM Store WHERE DistrictId = @DistrictId";

                    command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var store = new Store
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                DistrictId = reader.GetGuid(reader.GetOrdinal("DistrictId"))
                            };

                            stores.Add(store);
                        }
                    }
                }
            }

            return stores;
        }
    }
}
