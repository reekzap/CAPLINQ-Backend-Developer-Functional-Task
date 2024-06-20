namespace ShippingRatesAPI.Services.Interfaces
{
    public interface IShippingRateServiceFactory
    {
        IShippingRateService Create(string carrierName, string apiEndpoint);
    }
}
