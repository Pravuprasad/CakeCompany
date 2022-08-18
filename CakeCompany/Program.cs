// See https://aka.ms/new-console-template for more information

using CakeCompany.Models.Transport;
using CakeCompany.Provider.Cake;
using CakeCompany.Provider.Order;
using CakeCompany.Provider.Payment;
using CakeCompany.Provider.Shipment;
using CakeCompany.Provider.Time;
using CakeCompany.Provider.Transport;
using CakeCompany.Resolver;
using CakeCompany.Service.Order;
using CakeCompany.Service.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) => services
                .AddSingleton<Ship>()
                .AddSingleton<Truck>()
                .AddSingleton<Van>()
                .AddTransient<TransportResolver>(transportProvider => key =>
                {
                        return (key switch
                        {
                                "Ship" => transportProvider.GetService<Ship>(),
                                "Truck" => transportProvider.GetService<Truck>(),
                                "Van" => transportProvider.GetService<Van>(),
                                _ => throw new KeyNotFoundException()
                        })!;
                })
                .AddSingleton<ITransportService, TransportService>()
                .AddSingleton<IOrderService, OrderService>()
                .AddSingleton<ITransportProvider, TransportProvider>()
                .AddSingleton<ICakeProvider, CakeProvider>()
                .AddSingleton<IPaymentProvider, PaymentProvider>()
                .AddSingleton<IOrderProvider, OrderProvider>()
                .AddSingleton<ITimeProvider, TimeProvider>())
        .ConfigureLogging(logging =>
        {
                logging.ClearProviders();
                logging.AddConsole();
        })
        .Build();

GetShipment(host.Services);

await host.RunAsync();

void GetShipment(IServiceProvider services)
{
        using var serviceScope = services.CreateScope();
        var provider = serviceScope.ServiceProvider;
        var shipmentProvider = new ShipmentProvider(
                provider.GetRequiredService<IOrderProvider>(),
                provider.GetRequiredService<IOrderService>(),
                provider.GetRequiredService<ITransportService>(),
                provider.GetRequiredService<ILogger<Program>>());
        shipmentProvider.GetShipment();

        Console.WriteLine("Shipment Details...");
}

