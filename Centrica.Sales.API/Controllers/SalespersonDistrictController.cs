using Centrica.Sales.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Centrica.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalespersonDistrictController : ControllerBase
    {
        private readonly ISalespersonDistrictService _salespersonDistrictService;

        public SalespersonDistrictController(ISalespersonDistrictService salespersonDistrictService)
        {
            _salespersonDistrictService = salespersonDistrictService;
        }

        [HttpGet("{salespersonId}/{districtId}")]
        public async Task<IActionResult> Get(Guid salespersonId, Guid districtId)
        {
            var salespersonDistrict = await _salespersonDistrictService.GetSalespersonDistrictAsync(salespersonId, districtId);
            if (salespersonDistrict == null)
            {
                return NotFound();
            }
            return Ok(salespersonDistrict);
        }
    }
}
