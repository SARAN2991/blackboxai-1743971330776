using System.Threading.Tasks;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureAgentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/weather")]
    public class WeatherReportController : ControllerBase
    {
        private readonly WeatherReportAgent _weatherAgent;

        public WeatherReportController(WeatherReportAgent weatherAgent)
        {
            _weatherAgent = weatherAgent;
        }

        [HttpGet("{region}")]
        public async Task<IActionResult> GetCurrentWeather(string region)
        {
            try
            {
                var weather = await _weatherAgent.GetWeatherReportAsync(region);
                return Ok(weather);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("forecast/{region}/{days}")]
        public async Task<IActionResult> GetWeatherForecast(string region, int days)
        {
            try
            {
                // Implementation would fetch forecast data
                return Ok(new {
                    Region = region,
                    Days = days,
                    Message = "Weather forecast endpoint - implementation pending"
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}