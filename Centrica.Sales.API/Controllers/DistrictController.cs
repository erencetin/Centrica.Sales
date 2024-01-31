using Centrica.Sales.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Centrica.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var district = await _districtService.GetDistrictAsync(id);
            if (district == null)
            {
                return NotFound();
            }
            return Ok(district);
        }

        [HttpGet("with-salespersons-and-stores")]
        public async Task<IActionResult> GetDistrictsWithSalespersonsAndStores()
        {
            try
            {
                var districtsWithSalespersonsAndStores = await _districtService.GetDistrictsWithSalespersonsAndStoresAsync();
                return Ok(districtsWithSalespersonsAndStores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{districtId}/addSalesperson")]
        public async Task<IActionResult> AddSalespersonToDistrict(Guid districtId, bool isPrimary ,[FromBody] Guid salespersonId)
        {
            try
            {
                bool success = await _districtService.AssignSalespersonToDistrictAsync(districtId, salespersonId, isPrimary);
                if (success)
                {
                    return Ok("Salesperson added to district successfully.");
                }
                return BadRequest("Failed to add salesperson to district.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }
    }
}
