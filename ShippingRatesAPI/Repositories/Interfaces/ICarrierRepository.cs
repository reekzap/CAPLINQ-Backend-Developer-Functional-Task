using ShippingRatesAPI.Models;

namespace ShippingRatesAPI.Repositories.Interfaces
{
    public interface ICarrierRepository
    {
        Task<List<Carrier>> GetAllCarriersAsync();
        Task AddCarrierAsync(Carrier carrier);
        Task UpdateCarrierAsync(Carrier carrier);
        Task RemoveCarrierAsync(int carrierId);
        Task<string> DisableCarrierAsync(int carrierId, string reason);
        Task<string> EnableCarrierAsync(int carrierId);
    }
}
