namespace ShippingRatesAPI.Dto
{
    public class DhlRateRequestDto
    {
        public required DhlAddressInfo from { get; set; }
        public required DhlAddressInfo to { get; set; }
        public required DhlPackageInfo parcel { get; set; }
    }

    public class DhlAddressInfo
    {
        public required string zipCode { get; set; }
        public required string country { get; set; }
    }

    public class DhlPackageInfo
    {
        public int weightKg { get; set; }
        public required DhlDimensions sizeCm { get; set; }
    }

    public class DhlDimensions
    {
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
