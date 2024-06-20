using ShippingRatesAPI.Models;

namespace ShippingRatesAPI.Repositories.Interfaces
{
    public interface ICarrierDisableRequestRepository
    {
        Task<List<CarrierDisableRequest>> GetAllDisableRequestsAsync();
        Task<string> ApproveDisableRequestAsync(int requestId);
    }
}
