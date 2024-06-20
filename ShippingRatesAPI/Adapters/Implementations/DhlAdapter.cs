using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Dto;
using System.Text.Json;

namespace ShippingRatesAPI.Adapters.Implementations
{
    public class DhlAdapter : IDhlAdapter
    {
        public DhlRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request)
        {
            try
            {
                return new DhlRateRequestDto
                {
                    from = new DhlAddressInfo
                    {
                        zipCode = request.origin.postalCode,
                        country = request.origin.countryCode
                    },
                    to = new DhlAddressInfo
                    {
                        zipCode = request.destination.postalCode,
                        country = request.destination.countryCode
                    },
                    parcel = new DhlPackageInfo
                    {
                        weightKg = request.package.weightKg,
                        sizeCm = new DhlDimensions
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
                Console.WriteLine($"Error standardizing DHL request: {ex.Message}");
                throw;
            }
        }

        public ShippingRateResponseDto GetStandardizedResponse(string jsonResponse)
        {
            try
            {
                var dhlResponse = JsonSerializer.Deserialize<DhlRateResponseDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensure case insensitivity for properties
                });

                if (dhlResponse == null || dhlResponse.Options == null)
                {
                    throw new Exception("Invalid DHL API response format.");
                }

                var rateOptions = dhlResponse.Options.Select(option => new RateOption
                {
                    ServiceName = option.Name,
                    EstimatedDelivery = DateTime.Parse(option.DeliveryDate),
                    Price = new MoneyDto(option.Price, "USD")  // Assuming USD
                }).ToList();

                return new ShippingRateResponseDto
                {
                    Carrier = dhlResponse.Provider,
                    RateOptions = rateOptions
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error standardizing DHL response: {ex.Message}");
                throw;
            }
        }
    }
}
