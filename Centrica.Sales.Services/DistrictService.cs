using Centrica.Sales.Data.Interfaces;
using Centrica.Sales.Data.Model;
using Centrica.Sales.Services.Interfaces;

namespace Centrica.Sales.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IStoreRepository _storeRepository;

        public DistrictService(IDistrictRepository districtRepository, IStoreRepository storeRepository)
        {
            _districtRepository = districtRepository;
            _storeRepository = storeRepository;
        }

        public async Task<District> GetDistrictAsync(Guid id)
        {
            return await _districtRepository.GetByIdAsync(id);
        }

        public async Task<List<DistrictOverview>> GetDistrictsWithSalespersonsAndStoresAsync()
        {
            var districts = await _districtRepository.GetAllAsync();

            var districtOverviews = new List<DistrictOverview>();

            foreach (var district in districts)
            {
                var salespersons = await _districtRepository.GetSalespersonsByDistrictIdAsync(district.Id);
                var stores = await _storeRepository.GetStoresByDistrictIdAsync(district.Id);
                // Find the primary salesperson for the district
                var primarySalesperson = salespersons.FirstOrDefault(s => s.IsPrimary);

                // Populate the DistrictOverview with District Name and DistrictId
                var districtOverview = new DistrictOverview
                {
                    Id = district.Id,
                    Name = district.Name,
                    Salespersons = salespersons,
                    Stores = stores
                };

                // Set IsPrimary for the primary salesperson
                if (primarySalesperson != null)
                {
                    primarySalesperson.IsPrimary = true;
                }

                districtOverviews.Add(districtOverview);
            }

            return districtOverviews;
        }

        public async Task<bool> AssignSalespersonToDistrictAsync(Guid districtId, Guid salespersonId, bool isPrimarySalesperson)
        {

            // Check if the salesperson is already assigned to the same district
            bool isAlreadyAssigned = await _districtRepository.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId);

            if (isAlreadyAssigned)
            {
                // If the salesperson is already assigned to the district, return true
                return true;
            }

            // Check if there is already an assigned primary salesperson in same district
            bool alreadyHasAPrimarySalesperson = await _districtRepository.IsThereAPrimarySalespersonInDistrictAsync(districtId);
            if (!alreadyHasAPrimarySalesperson)
            {
                throw new Exception("A salesperson has been already assigned to this district.");
            }
            // Assign the salesperson to the district
            return await _districtRepository.AssignSalespersonToDistrictAsync(districtId, salespersonId, isPrimarySalesperson);
        }

        public async Task<bool> RemoveSalespersonFromDistrictAsync(Guid salespersonId, Guid districtId)
        {
            // Check if the salesperson is assigned to the district
            bool isAssignedToDistrict = await _districtRepository.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId);

            if (!isAssignedToDistrict)
            {
                // The salesperson is not assigned to the district, return false
                return false;
            }

            // Check if the salesperson is the primary salesperson of the district
            bool isPrimarySalesperson = await _districtRepository.IsPrimarySalespersonInDistrictAsync(salespersonId, districtId);

            if (isPrimarySalesperson)
            {
                throw new Exception("You should delete a primary salesperson. You can only change it.");             
            }

            // Remove the salesperson from the district
            return await _districtRepository.RemoveSalespersonFromDistrictAsync(salespersonId, districtId);
        }
    }
}
