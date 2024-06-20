namespace ShippingRatesAPI.Dto
{
    public class UpsRateRequestDto
    {
        public required UpsShipment shipment { get; set; }
    }

    public class UpsShipment
    {
        public required string originPostalCode { get; set; }
        public required string destinationPostalCode { get; set; }
        public required string originCountryCode { get; set; }
        public required string destinationCountryCode { get; set; }
        public int weightLbs { get; set; }
        public required UpsDimensions dimensionsInches { get; set; }
    }
    public class UpsDimensions
    {
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
