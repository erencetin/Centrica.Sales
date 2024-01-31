using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Data.Interfaces
{
    public interface ISalespersonDistrictRepository
    {
        Task<SalespersonDistrict> GetBySalespersonIdAndDistrictIdAsync(Guid salespersonId, Guid districtId);
    }


}
