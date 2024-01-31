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
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<Store> GetStoreAsync(Guid id)
        {
            return await _storeRepository.GetByIdAsync(id);
        }
    }
}
