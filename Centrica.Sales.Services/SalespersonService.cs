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
    public class SalespersonService : ISalespersonService
    {
        private readonly ISalespersonRepository _salespersonRepository;

        public SalespersonService(ISalespersonRepository salespersonRepository)
        {
            _salespersonRepository = salespersonRepository;
        }

        public async Task<Salesperson> GetSalespersonAsync(Guid id)
        {
            return await _salespersonRepository.GetByIdAsync(id);
        }
    }
}
