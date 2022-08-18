using CakeCompany.Models;
using CakeCompany.Provider.Time;

namespace CakeCompany.Provider.Cake;

internal class CakeProvider : ICakeProvider
{
    private readonly ITimeProvider _timeProvider;

    public CakeProvider(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTime Check(Models.Order order)
    {
        return order.Name switch
        {
            Models.Cake.Chocolate => _timeProvider.Now.Add(TimeSpan.FromMinutes(30)),
            Models.Cake.RedVelvet => _timeProvider.Now.Add(TimeSpan.FromMinutes(60)),
            _ => _timeProvider.Now.Add(TimeSpan.FromHours(15))
        };
    }

    public Product Bake(Models.Order order)
    {
        var product = new Product
        {
            Id = new Guid(),
            Quantity = order.Quantity
        };
        
        switch (order.Name)
        {
            case Models.Cake.Chocolate:
                product.Cake = Models.Cake.Chocolate;
                break;
            case Models.Cake.RedVelvet:
                product.Cake = Models.Cake.RedVelvet;
                break;
            case Models.Cake.Vanilla:
            default:
                product.Cake = Models.Cake.Vanilla;
                break;
        }

        return product;
    }
}