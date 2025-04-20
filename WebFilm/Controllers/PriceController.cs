using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Price;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Price>>> GetAll()
        {
            var prices = await _priceService.GetAll();
            return Ok(prices);
        }

        [HttpPost]
        public async Task<ActionResult<Price>> Create([FromBody] Price price)
        {
            var createdPrice = await _priceService.Create(price);
            return Ok(createdPrice);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _priceService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
} 