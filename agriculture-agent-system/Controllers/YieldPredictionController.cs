using System.Threading.Tasks;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureAgentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/yield-predictions")]
    public class YieldPredictionController : ControllerBase
    {
        private readonly YieldPredictionAgent _yieldAgent;

        public YieldPredictionController(YieldPredictionAgent yieldAgent)
        {
            _yieldAgent = yieldAgent;
        }

        [HttpPost("predict")]
        public async Task<IActionResult> PredictYield([FromBody] YieldPredictionRequest request)
        {
            try
            {
                var prediction = await _yieldAgent.PredictYieldAsync(
                    request.Region, 
                    request.Crop, 
                    request.SoilData);
                
                return Ok(new {
                    Region = request.Region,
                    Crop = request.Crop,
                    Prediction = prediction,
                    Units = "tons per hectare",
                    Confidence = "high" // Would come from actual prediction
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    public class YieldPredictionRequest
    {
        public string Region { get; set; }
        public string Crop { get; set; }
        public string SoilData { get; set; } // JSON string of soil parameters
    }
}