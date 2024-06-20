using ShippingRatesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ShippingRatesAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Carrier> Carriers { get; set; } = null!;
        public DbSet<CarrierDisableRequest> CarrierDisableRequests { get; set; } = null!;
    }
}
