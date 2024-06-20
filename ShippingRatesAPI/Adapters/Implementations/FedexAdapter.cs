using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Dto;
using System.Text.Json;

namespace ShippingRatesAPI.Adapters.Implementations
{
    public class FedexAdapter : IFedexAdapter
    {
        public FedexRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request)
        {
            try
            {
                return new FedexRateRequestDto
                {
                    origin = new FedexAddressInfo
                    {
                        postalCode = request.origin.postalCode,
                        countryCode = request.origin.countryCode
                    },
                    destination = new FedexAddressInfo
                    {
                        postalCode = request.destination.postalCode,
                        countryCode = request.destination.countryCode
                    },
                    package = new FedexPackageInfo
                    {
                        weight = request.package.weightKg,
                        dimensions = new FedexDimensions
                        {
                            length = request.package.dimensionsCm.length,
                            width = request.package.dimensionsCm.width,
                            height = request.package.dimensionsCm.height
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error standardizing FedEx request: {ex.Message}");
                throw;
            }
        }
        public ShippingRateResponseDto GetStandardizedResponse(string jsonResponse)
        {
            try
            {
                var fedexResponse = JsonSerializer.Deserialize<FedexRateResponseDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensure case insensitivity for properties
                });

                if (fedexResponse == null || fedexResponse.ServiceOptions == null)
                {
                    throw new Exception("Invalid DHL API response format.");
                }
                var rateOptions = fedexResponse.ServiceOptions.Select(option => new RateOption
                {
                    ServiceName = option.ServiceName,
                    EstimatedDelivery = DateTime.Parse(option.EstimatedDelivery),
                    Price = new MoneyDto(option.Rate, "USD")  // Assuming USD
                }).ToList();

                return new ShippingRateResponseDto
                {
                    Carrier = fedexResponse.Carrier,
                    RateOptions = rateOptions
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error standardizing FedEx response: {ex.Message}");
                throw;
            }
        }
    }
}
