namespace ShippingRatesAPI.Dto
{
    public class ShippingRateRequestDto
    {
        public required AddressInfo origin { get; set; }
        public required AddressInfo destination { get; set; }
        public required PackageInfo package { get; set; }
    }

    public class AddressInfo
    {
        public required string postalCode { get; set; }
        public required string countryCode { get; set; }
    }

    public class PackageInfo
    {
        public required int weightKg { get; set; }
        public required Dimensions dimensionsCm { get; set; }
    }

    public class Dimensions
    {
        public required int length { get; set; }
        public required int width { get; set; }
        public required int height { get; set; }
    }
}
