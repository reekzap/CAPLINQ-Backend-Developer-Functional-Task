using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Dto;
using ShippingRatesAPI.Services.Interfaces;
using ShippingRatesAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ShippingRatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingRatesController : ControllerBase
    {
        private readonly IShippingRateServiceFactory _shippingRateServiceFactory;
        private readonly DataContext _context;

        public ShippingRatesController(IShippingRateServiceFactory shippingRateServiceFactory, DataContext context)
        {
            _shippingRateServiceFactory = shippingRateServiceFactory;
            _context = context;
        }

        /// <summary>
        /// Returns Shipping Rates
        /// </summary>
        /// <param name="request">
        /// Request Payload
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// Expected Request:
        ///
        /// ```json
        /// {
        ///   "origin": {
        ///     "postalCode": "string",
        ///     "countryCode": "string"
        ///   },
        ///   "destination": {
        ///     "postalCode": "string",
        ///     "countryCode": "string"
        ///   },
        ///   "package": {
        ///     "weightKg": 0,
        ///     "dimensionsCm": {
        ///       "length": 0,
        ///       "width": 0,
        ///       "height": 0
        ///     }
        ///   }
        /// }
        /// ```
        ///
        /// Expected Response:
        ///
        /// ```json
        /// [
        ///   {
        ///     "carrier": "string",
        ///     "rateOptions": [
        ///       {
        ///         "serviceName": "string",
        ///         "estimatedDelivery": "DateTime",
        ///         "price": {
        ///           "amount": 0,
        ///           "currency": "string"
        ///         }
        ///       }
        ///     ]
        ///   }
        /// ]
        /// ```
        /// </remarks>
        [HttpPost("rates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPackageDetailsAsync([FromBody] ShippingRateRequestDto request)
        {
            var allResponses = new List<ShippingRateResponseDto>();
            var enabledCarriers = await _context.Carriers.Where(c => c.IsEnabled).ToListAsync();

            foreach (var carrier in enabledCarriers)
            {
                try
                {
                    var service = _shippingRateServiceFactory.Create(carrier.Name, carrier.ApiEndpoint);
                    var response = await service.GetShippingRateAsync(request);
                    allResponses.Add(response);
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Request Error: {ex.Message}");
                    // Optionally log the error or handle it as needed
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {carrier.Name} shipping rates: {ex.Message}");
                    return StatusCode(500, $"Error retrieving {carrier.Name} shipping rates.");
                    // If this is in a controller action, return a status code indicating a server error
                }
            }


            // Return all responses as a combined response
            return Ok(allResponses);
        }
    }
}
