using Centrica.Sales.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrica.Sales.Services.Interfaces
{
    public interface IStoreService
    {
        Task<Store> GetStoreAsync(Guid id);
    }
}
