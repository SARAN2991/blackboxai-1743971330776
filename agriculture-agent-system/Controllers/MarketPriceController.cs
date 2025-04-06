using System.Threading.Tasks;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureAgentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/market-prices")]
    public class MarketPriceController : ControllerBase
    {
        private readonly MarketPriceAgent _priceAgent;

        public MarketPriceController(MarketPriceAgent priceAgent)
        {
            _priceAgent = priceAgent;
        }

        [HttpGet("{crop}")]
        public async Task<IActionResult> GetCurrentPrice(string crop)
        {
            try
            {
                var price = await _priceAgent.GetCurrentPricesAsync(crop);
                return Ok(new { 
                    Crop = crop,
                    Price = price,
                    Currency = "INR",
                    LastUpdated = DateTime.UtcNow
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("trend/{crop}/{days}")]
        public async Task<IActionResult> GetPriceTrend(string crop, int days)
        {
            try
            {
                // Implementation would fetch historical data
                return Ok(new { 
                    Crop = crop,
                    Days = days,
                    Message = "Price trend endpoint - implementation pending"
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}