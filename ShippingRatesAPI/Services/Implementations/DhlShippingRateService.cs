using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Dto;
using ShippingRatesAPI.Services.Interfaces;
using System.Text;

namespace ShippingRatesAPI.Services.Implementations
{
    public class DhlShippingRateService : IShippingRateService
    {
        private readonly HttpClient _client;
        private readonly IDhlAdapter _dhlAdapter;
        private readonly IMemoryCache _memoryCache;

        public DhlShippingRateService(HttpClient client, IDhlAdapter dhlAdapter, IMemoryCache memoryCache)
        {
            _client = client;
            _dhlAdapter = dhlAdapter;
            _memoryCache = memoryCache;
        }

        public async Task<ShippingRateResponseDto> GetShippingRateAsync(ShippingRateRequestDto request)
        {
            var dhlStandardizedRequest = _dhlAdapter.GetStandardizedRequest(request);
            var jsonRequest = JsonConvert.SerializeObject(dhlStandardizedRequest);


            // Check if already cached
            var cacheKey = $"ShippingRateResponse-{jsonRequest}";
#pragma warning disable CS8600  // Converting null literal or possible null value to non-nullable type.
            if (_memoryCache.TryGetValue(cacheKey, out ShippingRateResponseDto cachedResponse))
            {
                // Log cache hit
                Console.WriteLine($"DHL - Cache hit for key: {cacheKey}");
#pragma warning disable CS8603 // Possible null reference return.
                return cachedResponse;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.


            //API Call
            var httpResponse = await _client.PostAsync("/api/dhl/rates", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var shippingRateResponse = _dhlAdapter.GetStandardizedResponse(jsonResponse);

            // Cache expiration of 5 minutes
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _memoryCache.Set(cacheKey, shippingRateResponse, cacheEntryOptions);
            // Log cache miss
            Console.WriteLine($"DHL - Cache miss for key: {cacheKey}");

            return shippingRateResponse;
        }
    }
}
