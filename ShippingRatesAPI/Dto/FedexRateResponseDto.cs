namespace ShippingRatesAPI.Dto
{
    public class FedexRateResponseDto
    {
        public required string Carrier { get; set; }
        public List<FedexServiceOption>? ServiceOptions { get; set; }
    }

    public class FedexServiceOption
    {
        public required string ServiceName { get; set; }
        public required string EstimatedDelivery { get; set; }
        public decimal Rate { get; set; }
    }
}
