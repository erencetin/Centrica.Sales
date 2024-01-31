using Centrica.Sales.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Centrica.Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var store = await _storeService.GetStoreAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }
    }
}
