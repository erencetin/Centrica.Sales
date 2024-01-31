using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<District> GetDistrictAsync(Guid id);
        Task<List<DistrictOverview>> GetDistrictsWithSalespersonsAndStoresAsync();
        Task<bool> AssignSalespersonToDistrictAsync(Guid districtId, Guid salespersonId, bool isPrimarySalesperson);
    }
}
