using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Dto;
using System.Text.Json;

namespace ShippingRatesAPI.Adapters.Implementations
{
    public class UpsAdapter : IUpsAdapter
    {
        static int ConvertKgToLbs(int kilograms)
        {
            const double conversionFactor = 2.20462;
            return (int)(kilograms * conversionFactor);
        }
        static int ConvertCmToInches(int centimeters)
        {
            const double conversionFactor = 0.393701;
            return (int)(centimeters * conversionFactor);
        }
        public UpsRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request)
        {
            try
            {
                return new UpsRateRequestDto
                {
                    shipment = new UpsShipment
                    {
                        originPostalCode = request.origin.postalCode,
                        destinationPostalCode = request.destination.postalCode,
                        originCountryCode = request.origin.countryCode,
                        destinationCountryCode = request.destination.countryCode,
                        weightLbs = ConvertKgToLbs(request.package.weightKg),
                        dimensionsInches = new UpsDimensions
                        {
                            length = ConvertCmToInches(request.package.dimensionsCm.length),
                            width = ConvertCmToInches(request.package.dimensionsCm.width),
                            height = ConvertCmToInches(request.package.dimensionsCm.height)
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error standardizing UPS request: {ex.Message}");
                throw;
            }
        }

        public ShippingRateResponseDto GetStandardizedResponse(string jsonResponse)
        {
            try
            {
                var upsResponse = JsonSerializer.Deserialize<UpsRateResponseDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensure case insensitivity for properties
                });

                if (upsResponse == null || upsResponse.Services == null)
                {
                    throw new Exception("Invalid UPS API response format.");
                }

                var rateOptions = upsResponse.Services.Select(option => new RateOption
                {
                    ServiceName = option.Service,
                    EstimatedDelivery = DateTime.Parse(option.Eta),
                    Price = new MoneyDto(option.Cost, "USD")  // Assuming USD
                }).ToList();

                return new ShippingRateResponseDto
                {
                    Carrier = upsResponse.Company,
                    RateOptions = rateOptions
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error standardizing UPS response: {ex.Message}");
                throw;
            }
        }
    }
}
