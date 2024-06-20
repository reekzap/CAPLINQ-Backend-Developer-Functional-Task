using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Add this attribute to ignore in Swagger
    [Route("api/[controller]")]
    [ApiController]
    public class dhlController : ControllerBase
    {
        [HttpPost("rates")]
        public IActionResult GetRates([FromBody] DhlRateRequestDto request)
        {
            try
            {
                // Process the request JObject if needed
                // For demonstration, returning a hardcoded response
                var jsonResponse = @"
                {
                    ""provider"": ""DHL"",
                    ""options"": [
                        {
                            ""name"": ""DHL Economy Select"",
                            ""deliveryDate"": ""2024-06-16"",
                            ""price"": 11.00
                        },
                        {
                            ""name"": ""DHL Express Worldwide"",
                            ""deliveryDate"": ""2024-06-10"",
                            ""price"": 22.50
                        },
                        {
                            ""name"": ""DHL Same Day"",
                            ""deliveryDate"": ""2024-06-09"",
                            ""price"": 35.00
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
