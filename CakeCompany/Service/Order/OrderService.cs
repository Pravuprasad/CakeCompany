using CakeCompany.Models;
using CakeCompany.Provider.Cake;
using CakeCompany.Provider.Payment;

namespace CakeCompany.Service.Order;

internal class OrderService :IOrderService
{
    private readonly ICakeProvider _cakeProvider;
    private readonly IPaymentProvider _paymentProvider;

    public OrderService(ICakeProvider cakeProvider, IPaymentProvider paymentProvider)
    {
        _cakeProvider = cakeProvider;
        _paymentProvider = paymentProvider;
    }

    public IEnumerable<Product> Process(ICollection<Models.Order> orders)
    {
        var cancelledOrders = new List<Models.Order>();
        var products = new List<Product>();

        foreach (var order in orders)
        {
            var estimatedBakeTime = _cakeProvider.Check(order);

            if (order.Quantity <= 0 ||
                estimatedBakeTime > order.EstimatedDeliveryTime ||
                !_paymentProvider.Process(order).IsSuccessful)
            {
                cancelledOrders.Add(order);
                continue;
            }

            var product = _cakeProvider.Bake(order);
            products.Add(product);
        }

        return products;
    }
}