using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Adapters.Interfaces
{
    public interface IFedexAdapter
    {
        FedexRateRequestDto GetStandardizedRequest(ShippingRateRequestDto request);
        ShippingRateResponseDto GetStandardizedResponse(string jsonResponse);
    }
}
