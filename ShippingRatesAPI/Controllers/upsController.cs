using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Add this attribute to ignore in Swagger
    [Route("api/[controller]")]
    [ApiController]
    public class upsController : ControllerBase
    {
        [HttpPost("rates")]
        public IActionResult GetRates([FromBody] UpsRateRequestDto request)
        {
            try
            {
                // Process the request JObject if needed
                // For demonstration, returning a hardcoded response
                var jsonResponse = @"
                {
                    ""company"": ""UPS"",
                    ""services"": [
                        {
                            ""service"": ""UPS Ground"",
                            ""eta"": ""2024-06-15"",
                            ""cost"": 15.20
                        },
                        {
                            ""service"": ""UPS 2nd Day Air"",
                            ""eta"": ""2024-06-11"",
                            ""cost"": 28.40
                        },
                        {
                            ""service"": ""UPS Next Day Air"",
                            ""eta"": ""2024-06-10"",
                            ""cost"": 52.75
                        }
                    ]
                }";

                return Ok(jsonResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
