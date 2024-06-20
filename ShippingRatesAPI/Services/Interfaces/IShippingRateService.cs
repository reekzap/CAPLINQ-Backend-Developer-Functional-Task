using ShippingRatesAPI.Dto;

namespace ShippingRatesAPI.Services.Interfaces
{
    public interface IShippingRateService
    {
        Task<ShippingRateResponseDto> GetShippingRateAsync(ShippingRateRequestDto request);
    }
}
