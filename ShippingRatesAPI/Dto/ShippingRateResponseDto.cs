namespace ShippingRatesAPI.Dto
{
    public record ShippingRateResponseDto
    {
        public required string Carrier { get; init; }
        public IEnumerable<RateOption>? RateOptions { get; init; }
    }

    public record RateOption
    {
        public required string ServiceName { get; init; }
        public required DateTime EstimatedDelivery { get; init; }
        public required MoneyDto Price { get; init; }
    }

    public record MoneyDto(decimal Amount, string Currency);
}
