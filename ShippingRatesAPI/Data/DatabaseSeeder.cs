using ShippingRatesAPI.Models;

namespace ShippingRatesAPI.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                context.Database.EnsureCreated();

                if (!context.Carriers.Any())
                {
                    context.Carriers.AddRange(
                        new Carrier { Id = 1, Name = "FedEx", ApiEndpoint = "https://localhost:7013/", ApiKey = "FedExApiKey" },
                        new Carrier { Id = 2, Name = "DHL", ApiEndpoint = "https://localhost:7013/", ApiKey = "DHLApiKey" },
                        new Carrier { Id = 3, Name = "UPS", ApiEndpoint = "https://localhost:7013/", ApiKey = "UPSApiKey" }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
