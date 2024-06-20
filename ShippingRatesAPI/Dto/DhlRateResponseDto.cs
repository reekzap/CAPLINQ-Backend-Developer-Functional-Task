namespace ShippingRatesAPI.Dto
{
    public class DhlRateResponseDto
    {
        public required string Provider { get; set; }
        public List<DhlOption>? Options { get; set; }
    }

    public class DhlOption
    {
        public required string Name { get; set; }
        public required string DeliveryDate { get; set; }
        public decimal Price { get; set; }
    }
}
