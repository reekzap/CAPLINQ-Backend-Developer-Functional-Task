using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Dto;
using ShippingRatesAPI.Services.Interfaces;
using System.Text;

namespace ShippingRatesAPI.Services.Implementations
{
    public class UpsShippingRateService : IShippingRateService
    {
        private readonly HttpClient _client;
        private readonly IUpsAdapter _upsAdapter;
        private readonly IMemoryCache _memoryCache;

        public UpsShippingRateService(HttpClient client, IUpsAdapter upsAdapter, IMemoryCache memoryCache)
        {
            _client = client;
            _upsAdapter = upsAdapter;
            _memoryCache = memoryCache;
        }

        public async Task<ShippingRateResponseDto> GetShippingRateAsync(ShippingRateRequestDto request)
        {
            var upsStandardizedRequest = _upsAdapter.GetStandardizedRequest(request);
            var jsonRequest = JsonConvert.SerializeObject(upsStandardizedRequest);


            // Check if already cached
            var cacheKey = $"ShippingRateResponse-{jsonRequest}";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            if (_memoryCache.TryGetValue(cacheKey, out ShippingRateResponseDto cachedResponse))
            {
                // Log cache hit
                Console.WriteLine($"UPS - Cache hit for key: {cacheKey}");
#pragma warning disable CS8603 // Possible null reference return.
                return cachedResponse;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.


            //API Call
            var httpResponse = await _client.PostAsync("/api/ups/rates", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var shippingRateResponse = _upsAdapter.GetStandardizedResponse(jsonResponse);

            // Cache expiration of 5 minutes
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _memoryCache.Set(cacheKey, shippingRateResponse, cacheEntryOptions);
            // Log cache miss
            Console.WriteLine($"UPS - Cache miss for key: {cacheKey}");

            return shippingRateResponse;
        }
    }
}
