namespace ShippingRatesAPI.Dto
{
    public class FedexRateRequestDto
    {
        public required FedexAddressInfo origin { get; set; }
        public required FedexAddressInfo destination { get; set; }
        public required FedexPackageInfo package { get; set; }
    }

    public class FedexAddressInfo
    {
        public required string postalCode { get; set; }
        public required string countryCode { get; set; }
    }

    public class FedexPackageInfo
    {
        public int weight { get; set; }
        public required FedexDimensions dimensions { get; set; }
    }

    public class FedexDimensions
    {
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
