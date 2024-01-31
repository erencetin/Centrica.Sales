using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Services.Interfaces
{
    public interface ISalespersonService
    {
        Task<Salesperson> GetSalespersonAsync(Guid id);
        // Additional methods as needed (e.g., Create, Update, Delete)
    }
}
