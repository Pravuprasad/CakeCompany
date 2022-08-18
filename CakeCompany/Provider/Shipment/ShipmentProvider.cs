using CakeCompany.Provider.Order;
using CakeCompany.Service.Order;
using CakeCompany.Service.Transport;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Provider.Shipment;

internal class ShipmentProvider
{
    private readonly IOrderProvider _orderProvider;
    private readonly IOrderService _orderService;
    private readonly ITransportService _transportService;
    private readonly ILogger _logger;
    
    public ShipmentProvider(
        IOrderProvider orderProvider, 
        IOrderService orderService, 
        ITransportService transportService,
        ILogger logger)
    {
        _orderProvider = orderProvider;
        _orderService = orderService;
        _transportService = transportService;
        _logger = logger;
    }

    public void GetShipment()
    {
        _logger.LogTrace("Entering method: {Class}.{Method}", nameof(GetShipment), nameof(ShipmentProvider));
        try
        {
            //Call order to get new orders
            _logger.LogInformation("Getting new orders");
            var orders = _orderProvider.GetLatestOrders();

            if (!orders.Any())
            {
                _logger.LogInformation("No new orders found");
                return;
            }
        
            _logger.LogInformation("Processing products");
            var products = _orderService.Process(orders).ToList();
        
            if (!products.Any())
            {
                _logger.LogInformation("No products found");
                return;
            }
            
            _logger.LogInformation("Delivering products");
            _transportService.Deliver(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while getting shipment");
        }
    }
}
