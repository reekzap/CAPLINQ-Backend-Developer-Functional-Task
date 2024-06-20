namespace ShippingRatesAPI.Models
{
    public class Carrier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ApiEndpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public bool IsEnabled { get; private set; } = true;
        public bool hasOngoingShipments { get; set; } = false;
        public bool hasPendingInvoices { get; set; } = false;

        public void EnableCarrier()
        {
            IsEnabled = true;
        }
        public void DisableCarrier()
        {
            IsEnabled = false;
        }

        public bool CanBeDisabled(string carrierName, string reason, bool isOnlyActiveCarrier, bool hasOngoingShipments, bool hasPendingInvoices)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new InvalidOperationException($"Carrier {carrierName} cannot be disabled without a reason.");
            }

            if (isOnlyActiveCarrier)
            {
                throw new InvalidOperationException($"Carrier {carrierName} cannot be disabled because it is the only active carrier.");
            }

            if (hasOngoingShipments)
            {
                throw new InvalidOperationException($"Carrier {carrierName} has ongoing shipments and cannot be disabled.");
            }

            if (hasPendingInvoices)
            {
                throw new InvalidOperationException($"Carrier {carrierName} has pending invoices and cannot be disable.");
            }

            return true;
        }
    }
}
