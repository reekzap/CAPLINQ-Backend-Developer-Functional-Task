using Microsoft.EntityFrameworkCore;
using ShippingRatesAPI.Data;
using ShippingRatesAPI.Models;
using ShippingRatesAPI.Repositories.Interfaces;

namespace ShippingRatesAPI.Repositories.Implementations
{
    public class CarrierDisableRequestRepository : ICarrierDisableRequestRepository
    {
        private readonly DataContext _context;

        public CarrierDisableRequestRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<List<CarrierDisableRequest>> GetAllDisableRequestsAsync() // Implement the method
        {
            return await _context.CarrierDisableRequests.ToListAsync();
        }

        public async Task<string> ApproveDisableRequestAsync(int requestId)
        {
            var disableRequest = await _context.CarrierDisableRequests.FindAsync(requestId) ?? throw new InvalidOperationException("Request not found.");
            int carrierId = disableRequest.CarrierId;
            string reason = disableRequest.Reason;

            var carrier = await _context.Carriers.FindAsync(carrierId) ?? throw new InvalidOperationException("Carrier not found.");

            if (!carrier.IsEnabled)
            {
                throw new InvalidOperationException("Carrier is already disabled.");
            }

            var isOnlyActiveCarrier = _context.Carriers.Count(c => c.IsEnabled) == 1;
            bool hasPendingInvoices = carrier.hasPendingInvoices;
            bool hasOngoingShipments = carrier.hasOngoingShipments;
            string carrierName = carrier.Name;


            bool canBeDisable = carrier.CanBeDisabled(carrierName, reason, isOnlyActiveCarrier, hasOngoingShipments, hasPendingInvoices);
            if (canBeDisable)
            {
                carrier.DisableCarrier();
                await _context.SaveChangesAsync();

                disableRequest.ApproveRequest();
                await _context.SaveChangesAsync();

                return $"The request to disable the carrier was approved. The carrier {carrierName} is now disabled.";
            }
            else
            {
                throw new InvalidOperationException("Cannot disable a carrier for some reasons.");
            }
        }
    }
}
