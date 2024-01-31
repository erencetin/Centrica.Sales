using Centrica.Sales.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Centrica.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalespersonController : ControllerBase
    {
        private readonly ISalespersonService _salespersonService;

        public SalespersonController(ISalespersonService salespersonService)
        {
            _salespersonService = salespersonService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var salesperson = await _salespersonService.GetSalespersonAsync(id);
            if (salesperson == null)
            {
                return NotFound();
            }
            return Ok(salesperson);
        }
    }
}
