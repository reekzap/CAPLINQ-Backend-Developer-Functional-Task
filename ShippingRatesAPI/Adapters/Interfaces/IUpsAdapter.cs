using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Adapters.Interfaces
{
    public interface IUpsAdapter
    {
        UpsRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request);
        ShippingRateResponseDto GetStandardizedResponse(string jsonResponse);
    }
}
