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
    public class DistrictRepository : BaseRepository, IDistrictRepository
    {
        public DistrictRepository(string connectionString) : base(connectionString) { }

        public async Task<District> GetByIdAsync(Guid id)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, [Name] FROM District WHERE Id = @Id";
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = id });
                var dbCommand = (DbCommand)command;
                using (var reader = await dbCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new District
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<List<District>> GetAllAsync()
        {
            var districts = new List<District>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Id, [Name] FROM District";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var district = new District
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            districts.Add(district);
                        }
                    }
                }
            }

            return districts;
        }

        public async Task<List<Salesperson>> GetSalespersonsByDistrictIdAsync(Guid districtId)
        {
            var salespersons = new List<Salesperson>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT s.Id, s.FirstName, s.LastName, sd.IsPrimary " +
                                          "FROM Salesperson s " +
                                          "JOIN SalespersonDistrict sd ON s.Id = sd.SalespersonId " +
                                          "WHERE sd.DistrictId = @DistrictId";

                    command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var salesperson = new Salesperson
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                IsPrimary = reader.GetBoolean(reader.GetOrdinal("IsPrimary"))
                            };

                            salespersons.Add(salesperson);
                        }
                    }
                }
            }

            return salespersons;
        }

        public async Task<bool> AssignSalespersonToDistrictAsync(Guid districtId, Guid salespersonId, bool isPrimarySalesperson)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = (DbCommand)connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO SalespersonDistrict (SalespersonId, DistrictId, IsPrimary)
                    VALUES (@SalespersonId, @DistrictId, @IsPrimary)";
                command.Parameters.Add(new SqlParameter("@SalespersonId", SqlDbType.UniqueIdentifier) { Value = salespersonId });
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });
                command.Parameters.Add(new SqlParameter("@IsPrimary", SqlDbType.Bit) { Value = isPrimarySalesperson });

                int affectedRows = await command.ExecuteNonQueryAsync();

                return affectedRows > 0; // Return true if the insert was successful
            }
        }

        public async Task<bool> IsThereAPrimarySalespersonInDistrictAsync(Guid districtId)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = (DbCommand)connection.CreateCommand();
                command.CommandText = @"
                    SELECT TOP 1 1
                    FROM SalespersonDistrict
                    WHERE DistrictId = @DistrictId AND IsPrimary = 1";
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });

                var result = await command.ExecuteScalarAsync();

                return result != null; // If a result is found, there is a primary salesperson in the district
            }
        }
        public async Task<bool> IsPrimarySalespersonInDistrictAsync(Guid districtId, Guid salespersonId)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = (DbCommand)connection.CreateCommand();
                command.CommandText = @"
                    SELECT TOP 1 1
                    FROM SalespersonDistrict
                    WHERE DistrictId = @DistrictId AND IsPrimary = 1 AND SalespersonId = @SalespersonId";
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });
                command.Parameters.Add(new SqlParameter("@SalespersonId", SqlDbType.UniqueIdentifier) { Value = salespersonId });


                var result = await command.ExecuteScalarAsync();

                return result != null; // If a result is found, there is a primary salesperson in the district
            }
        }

        public async Task<bool> IsSalespersonAssignedToDistrictAsync(Guid salespersonId, Guid districtId)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = (DbCommand)connection.CreateCommand();
                command.CommandText = @"
                SELECT TOP 1 1
                FROM SalespersonDistrict
                WHERE SalespersonId = @SalespersonId AND DistrictId = @DistrictId";
                command.Parameters.Add(new SqlParameter("@SalespersonId", SqlDbType.UniqueIdentifier) { Value = salespersonId });
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });

                var result = await command.ExecuteScalarAsync();

                return result != null; // If a result is found, the salesperson is assigned to the district
            }
        }


        public async Task<bool> RemoveSalespersonFromDistrictAsync(Guid salespersonId, Guid districtId)
        {
            using (var connection = await CreateConnectionAsync())
            {
                var command = (DbCommand)connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM SalespersonDistrict
                    WHERE SalespersonId = @SalespersonId AND DistrictId = @DistrictId";
                command.Parameters.Add(new SqlParameter("@SalespersonId", SqlDbType.UniqueIdentifier) { Value = salespersonId });
                command.Parameters.Add(new SqlParameter("@DistrictId", SqlDbType.UniqueIdentifier) { Value = districtId });

                var rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }
    }
}
