
using Centrica.Sales.Data.Interfaces;
using Centrica.Sales.Data.Model;
using Centrica.Sales.Services;
using Moq;
using NUnit.Framework;

namespace Centrica.Sales.Test.Services
{


    [TestFixture]
    public class DistrictServiceTests
    {
        private Mock<IDistrictRepository> _mockDistrictRepository;
        private Mock<IStoreRepository> _mockStoreRepository;
        private DistrictService _districtService;

        [SetUp]
        public void Setup()
        {
            _mockDistrictRepository = new Mock<IDistrictRepository>();
            _mockStoreRepository = new Mock<IStoreRepository>();
            _districtService = new DistrictService(_mockDistrictRepository.Object, _mockStoreRepository.Object);
        }

        [Test]
        public async Task GetDistrictAsync_ReturnsDistrict_WhenIdIsValid()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var expectedDistrict = new District { Id = districtId };
            _mockDistrictRepository.Setup(repo => repo.GetByIdAsync(districtId)).ReturnsAsync(expectedDistrict);

            // Act
            var result = await _districtService.GetDistrictAsync(districtId);

            // Assert
            Assert.AreEqual(expectedDistrict, result);
        }

        [Test]
        public async Task GetDistrictAsync_ReturnsNull_WhenIdIsInvalid()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.GetByIdAsync(districtId)).ReturnsAsync((District)null);

            // Act
            var result = await _districtService.GetDistrictAsync(districtId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetDistrictsWithSalespersonsAndStoresAsync_ReturnsDistrictOverviews_WhenDistrictsExist()
        {
            // Arrange
            var districts = new List<District>
        {
            new District { Id = Guid.NewGuid(), Name = "District 1" },
            new District { Id = Guid.NewGuid(), Name = "District 2" }
        };
            _mockDistrictRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(districts);
            _mockDistrictRepository.Setup(repo => repo.GetSalespersonsByDistrictIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(new List<Salesperson>());
            _mockStoreRepository.Setup(repo => repo.GetStoresByDistrictIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(new List<Store>());

            // Act
            var result = await _districtService.GetDistrictsWithSalespersonsAndStoresAsync();

            // Assert
            Assert.AreEqual(districts.Count, result.Count);
            Assert.IsTrue(result.All(d => districts.Any(dt => dt.Id == d.Id && dt.Name == d.Name)));
        }

        [Test]
        public async Task GetDistrictsWithSalespersonsAndStoresAsync_ReturnsEmptyList_WhenNoDistrictsExist()
        {
            // Arrange
            _mockDistrictRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<District>());

            // Act
            var result = await _districtService.GetDistrictsWithSalespersonsAndStoresAsync();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task AssignSalespersonToDistrictAsync_ReturnsTrue_WhenSuccessfullyAssigned()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(false);
            _mockDistrictRepository.Setup(repo => repo.IsThereAPrimarySalespersonInDistrictAsync(districtId)).ReturnsAsync(true);
            _mockDistrictRepository.Setup(repo => repo.AssignSalespersonToDistrictAsync(districtId, salespersonId, It.IsAny<bool>())).ReturnsAsync(true);

            // Act
            var result = await _districtService.AssignSalespersonToDistrictAsync(districtId, salespersonId, false);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AssignSalespersonToDistrictAsync_ReturnsTrue_WhenAlreadyAssigned()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(true);

            // Act
            var result = await _districtService.AssignSalespersonToDistrictAsync(districtId, salespersonId, false);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AssignSalespersonToDistrictAsync_ThrowsException_WhenPrimaryAlreadyExists()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(false);
            _mockDistrictRepository.Setup(repo => repo.IsThereAPrimarySalespersonInDistrictAsync(districtId)).ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _districtService.AssignSalespersonToDistrictAsync(districtId, salespersonId, true));
        }

        [Test]
        public async Task RemoveSalespersonFromDistrictAsync_ReturnsTrue_WhenSuccessfullyRemoved()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(true);
            _mockDistrictRepository.Setup(repo => repo.IsPrimarySalespersonInDistrictAsync(salespersonId, districtId)).ReturnsAsync(false);
            _mockDistrictRepository.Setup(repo => repo.RemoveSalespersonFromDistrictAsync(salespersonId, districtId)).ReturnsAsync(true);

            // Act
            var result = await _districtService.RemoveSalespersonFromDistrictAsync(salespersonId, districtId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveSalespersonFromDistrictAsync_ReturnsFalse_WhenNotAssigned()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(false);

            // Act
            var result = await _districtService.RemoveSalespersonFromDistrictAsync(salespersonId, districtId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveSalespersonFromDistrictAsync_ThrowsException_WhenPrimarySalesperson()
        {
            // Arrange
            var districtId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            _mockDistrictRepository.Setup(repo => repo.IsSalespersonAssignedToDistrictAsync(salespersonId, districtId)).ReturnsAsync(true);
            _mockDistrictRepository.Setup(repo => repo.IsPrimarySalespersonInDistrictAsync(salespersonId, districtId)).ReturnsAsync(true);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _districtService.RemoveSalespersonFromDistrictAsync(salespersonId, districtId));
        }
    }

}
