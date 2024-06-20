namespace ShippingRatesAPI.Dto
{
    public class UpsRateResponseDto
    {
        public required string Company { get; set; }
        public List<UpsService>? Services { get; set; }
    }

    public class UpsService
    {
        public required string Service { get; set; }
        public required string Eta { get; set; }
        public decimal Cost { get; set; }
    }
}
