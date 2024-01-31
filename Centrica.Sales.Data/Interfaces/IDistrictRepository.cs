using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Interfaces
{
    public interface IDistrictRepository
    {
        Task<District> GetByIdAsync(Guid id);
        Task<List<District>> GetAllAsync();
        Task<List<Salesperson>> GetSalespersonsByDistrictIdAsync(Guid districtId);
        Task<bool> AssignSalespersonToDistrictAsync(Guid districtId, Guid salespersonId, bool isPrimarySalesperson);
        Task<bool> IsThereAPrimarySalespersonInDistrictAsync(Guid districtId);
        Task<bool> IsSalespersonAssignedToDistrictAsync(Guid salespersonId, Guid districtId);
        Task<bool> RemoveSalespersonFromDistrictAsync(Guid salespersonId, Guid districtId);
        Task<bool> IsPrimarySalespersonInDistrictAsync(Guid districtId, Guid salespersonId);
    }
}

