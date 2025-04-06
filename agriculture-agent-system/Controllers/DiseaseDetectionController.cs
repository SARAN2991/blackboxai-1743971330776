using System.Threading.Tasks;
using AgricultureAgentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgricultureAgentSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/disease-detection")]
    public class DiseaseDetectionController : ControllerBase
    {
        private readonly DiseaseDetectionAgent _diseaseAgent;

        public DiseaseDetectionController(DiseaseDetectionAgent diseaseAgent)
        {
            _diseaseAgent = diseaseAgent;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectDisease([FromForm] DiseaseDetectionRequest request)
        {
            try
            {
                if (request.ImageFile == null || request.ImageFile.Length == 0)
                    return BadRequest(new { Message = "No image file provided" });

                // Save the file temporarily (in production would use cloud storage)
                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                var result = await _diseaseAgent.DetectDiseaseAsync(filePath, request.PlantType);
                
                // Clean up temp file
                System.IO.File.Delete(filePath);

                return Ok(new {
                    PlantType = request.PlantType,
                    DetectionResult = result,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    public class DiseaseDetectionRequest
    {
        public string PlantType { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}