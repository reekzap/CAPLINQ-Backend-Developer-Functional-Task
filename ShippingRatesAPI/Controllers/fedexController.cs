using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // Add this attribute to ignore in Swagger
    [Route("api/[controller]")]
    [ApiController]
    public class fedexController : ControllerBase
    {

        [HttpPost("rates")]
        public IActionResult GetRates([FromBody] FedexRateRequestDto request)
        {
            try
            {
                // Process the request JObject if needed
                // For demonstration, returning a hardcoded response
                var jsonResponse = @"
                {
                    ""carrier"": ""FedEx"",
                    ""serviceOptions"": [
                        {
                            ""serviceName"": ""FedEx Ground"",
                            ""estimatedDelivery"": ""2024-06-15"",
                            ""rate"": 12.34
                        },
                        {
                            ""serviceName"": ""FedEx 2Day"",
                            ""estimatedDelivery"": ""2024-06-10"",
                            ""rate"": 25.67
                        },
                        {
                            ""serviceName"": ""FedEx Overnight"",
                            ""estimatedDelivery"": ""2024-06-09"",
                            ""rate"": 45.89
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
