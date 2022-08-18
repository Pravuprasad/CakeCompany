using CakeCompany.Models;
using CakeCompany.Provider.Transport;
using CakeCompany.Resolver;

namespace CakeCompany.Service.Transport;

internal class TransportService : ITransportService
{
    private readonly ITransportProvider _transportProvider;
    private readonly TransportResolver _transportResolver;

    public TransportService(ITransportProvider transportProvider, TransportResolver transportResolver)
    {
        _transportProvider = transportProvider;
        _transportResolver = transportResolver;
    }

    public void Deliver(List<Product> products)
    {
        var transportName = _transportProvider.CheckForAvailability(products);

        var transport = _transportResolver(transportName);
        
        transport.Deliver(products);
    }
}