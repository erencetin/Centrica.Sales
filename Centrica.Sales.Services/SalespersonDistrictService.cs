using Centrica.Sales.Data.Interfaces;
using Centrica.Sales.Data.Model;
using Centrica.Sales.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Services
{
    public class SalespersonDistrictService : ISalespersonDistrictService
    {
        private readonly ISalespersonDistrictRepository _salespersonDistrictRepository;

        public SalespersonDistrictService(ISalespersonDistrictRepository salespersonDistrictRepository)
        {
            _salespersonDistrictRepository = salespersonDistrictRepository;
        }

        public async Task<SalespersonDistrict> GetSalespersonDistrictAsync(Guid salespersonId, Guid districtId)
        {
            return await _salespersonDistrictRepository.GetBySalespersonIdAndDistrictIdAsync(salespersonId, districtId);
        }
    }
}
