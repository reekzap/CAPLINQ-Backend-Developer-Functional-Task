namespace ShippingRatesAPI.Models
{
    public class CarrierDisableRequest
    {
        public int Id { get; set; }
        public int CarrierId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public bool IsApproved { get; private set; } = false;


        public void ApproveRequest()
        {
            IsApproved = true;
        }
    }
}
