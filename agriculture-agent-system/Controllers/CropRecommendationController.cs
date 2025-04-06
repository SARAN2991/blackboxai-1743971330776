using System.Threading.Tasks;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureAgentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CropRecommendationController : ControllerBase
    {
        private readonly CropRecommendationAgent _cropAgent;

        public CropRecommendationController(CropRecommendationAgent cropAgent)
        {
            _cropAgent = cropAgent;
        }

        [HttpGet("recommend/{region}")]
        public async Task<IActionResult> GetRecommendations(string region)
        {
            try
            {
                var recommendations = await _cropAgent.RecommendCropsAsync(region);
                return Ok(recommendations);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}