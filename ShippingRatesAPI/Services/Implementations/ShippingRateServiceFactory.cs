using ShippingRatesAPI.Services.Interfaces;

namespace ShippingRatesAPI.Services.Implementations
{
    public class ShippingRateServiceFactory : IShippingRateServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _serviceMappings;

        public ShippingRateServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceMappings = new Dictionary<string, Type>
            {
                { "FedEx", typeof(FedexShippingRateService) },
                { "DHL", typeof(DhlShippingRateService) },
                { "UPS", typeof(UpsShippingRateService) }
            };
        }

        public IShippingRateService Create(string carrierName, string apiEndpoint)
        {
            if (_serviceMappings.TryGetValue(carrierName, out var serviceType))
            {
                var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(apiEndpoint);

                var service = (IShippingRateService)ActivatorUtilities.CreateInstance(_serviceProvider, serviceType, httpClient);
                return service;
            }

            throw new ArgumentException($"No service found for carrier: {carrierName}", nameof(carrierName));
        }
    }
}
