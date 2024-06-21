using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShippingRatesAPI.Adapters.Implementations;
using ShippingRatesAPI.Adapters.Interfaces;
using ShippingRatesAPI.Data;
using ShippingRatesAPI.Repositories.Implementations;
using ShippingRatesAPI.Repositories.Interfaces;
using ShippingRatesAPI.Services.Implementations;
using ShippingRatesAPI.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// DBContext
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseInMemoryDatabase("CarrierAPIList"));

// Add HTTP Client
builder.Services.AddHttpClient<FedexShippingRateService>();
builder.Services.AddHttpClient<DhlShippingRateService>();
builder.Services.AddHttpClient<UpsShippingRateService>();

// Initialize DI with scoped and transient services
builder.Services.AddScoped<IShippingRateServiceFactory, ShippingRateServiceFactory>();
builder.Services.AddTransient<ICarrierRepository, CarrierRepository>();
builder.Services.AddTransient<ICarrierDisableRequestRepository, CarrierDisableRequestRepository>();
builder.Services.AddTransient<IFedexAdapter, FedexAdapter>();
builder.Services.AddTransient<IDhlAdapter, DhlAdapter>();
builder.Services.AddTransient<IUpsAdapter, UpsAdapter>();

// Register ShippingRateService instances with their respective implementations
builder.Services.AddTransient<IShippingRateService, FedexShippingRateService>();
builder.Services.AddTransient<IShippingRateService, DhlShippingRateService>();
builder.Services.AddTransient<IShippingRateService, UpsShippingRateService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Carrier rates backend developer assessment",
        Description = "Create a C# Web API that fetches shipping rates from various carriers. Users can input package details and destination to get rates, which will be standardized and returned in a clear format",
        License = new OpenApiLicense
        {
            Name = "Ricardo Zapanta Jr.",
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Seed the in-memory database.
DatabaseSeeder.SeedData(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
