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
    public class SalespersonDistrictRepository : BaseRepository, ISalespersonDistrictRepository
    {
        public SalespersonDistrictRepository(string connectionString) : base(connectionString) { }

        public async Task<SalespersonDistrict> GetBySalespersonIdAndDistrictIdAsync(Guid salespersonId, Guid districtId)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT SalespersonId, DistrictId, IsPrimary FROM SalespersonDistrict WHERE SalespersonId = @SalespersonId AND DistrictId = @DistrictId";
                command.Parameters.Add(new SqlParameter("@SalespersonId", SqlDbType.UniqueIdentifier) { Value = salespersonId });
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });
                var dbCommand = (DbCommand)command;
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new SalespersonDistrict
                        {
                            SalespersonId = reader.GetGuid(reader.GetOrdinal("SalespersonId")),
                            DistrictId = reader.GetGuid(reader.GetOrdinal("DistrictId")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("IsPrimary"))
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
