using Microsoft.EntityFrameworkCore;
using ShippingRatesAPI.Data;
using ShippingRatesAPI.Models;
using ShippingRatesAPI.Repositories.Interfaces;

namespace ShippingRatesAPI.Repositories.Implementations
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly DataContext _context;

        public CarrierRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Carrier>> GetAllCarriersAsync()
        {
            return await _context.Carriers.ToListAsync();
        }

        public async Task AddCarrierAsync(Carrier carrier)
        {
            _context.Carriers.Add(carrier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarrierAsync(Carrier carrier)
        {
            _context.Carriers.Update(carrier);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCarrierAsync(int carrierId)
        {
            var carrier = await _context.Carriers.FindAsync(carrierId) ?? throw new InvalidOperationException("Carrier not found.");
            _context.Carriers.Remove(carrier);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DisableCarrierAsync(int carrierId, string reason)
        {
            var carrier = await _context.Carriers.FindAsync(carrierId) ?? throw new InvalidOperationException("Carrier not found.");

            if (!carrier.IsEnabled)
            {
                throw new InvalidOperationException("Carrier is already disabled.");
            }

            var isOnlyActiveCarrier = _context.Carriers.Count(c => c.IsEnabled) == 1;
            bool hasPendingInvoices = carrier.hasPendingInvoices;
            bool hasOngoingShipments = carrier.hasOngoingShipments;
            var isAdmin = false; // Add logic to check for user auth
            string carrierName = carrier.Name;

            bool canBeDisable = carrier.CanBeDisabled(carrierName, reason, isOnlyActiveCarrier, hasOngoingShipments, hasPendingInvoices);
            if (canBeDisable)
            {
                if (isAdmin)
                {
                    // Disable the carrier
                    carrier.DisableCarrier();
                    await _context.SaveChangesAsync();

                    return $"Carrier {carrierName} disabled successfully.";
                }
                else
                {
                    // Send request to admin
                    var disableRequest = new CarrierDisableRequest
                    {
                        CarrierId = carrierId,
                        Reason = reason,
                        RequestedAt = DateTime.UtcNow
                    };
                    _context.CarrierDisableRequests.Add(disableRequest);
                    await _context.SaveChangesAsync();

                    return "Only an admin can disable a carrier. We will send a request to the admin and wait for approval";
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot disable a carrier for some reasons.");
            }
        }

        public async Task<string> EnableCarrierAsync(int carrierId)
        {
            var carrier = await _context.Carriers.FindAsync(carrierId) ?? throw new InvalidOperationException("Carrier not found.");
            carrier.EnableCarrier();
            await _context.SaveChangesAsync();

            return "Carrier enabled successfully.";
        }
    }
}
