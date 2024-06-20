using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Adapters.Interfaces
{
    public interface IDhlAdapter
    {
        DhlRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request);
        ShippingRateResponseDto GetStandardizedResponse(string jsonResponse);
    }
}
