using CakeCompany.Provider.Time;

namespace CakeCompany.Provider.Order;

internal class OrderProvider : IOrderProvider
{
    private readonly ITimeProvider _timeProvider;

    public OrderProvider(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }
    
    public Models.Order[] GetLatestOrders()
    {
        return new Models.Order[]
        {
            new("CakeBox", _timeProvider.Now, 1, Models.Cake.RedVelvet, 120.25),
            new("ImportantCakeShop", _timeProvider.Now, 1, Models.Cake.RedVelvet, 120.25),
            new("ImportantCakeShop", _timeProvider.Now.AddDays(1), 1, Models.Cake.RedVelvet, 120.25),
            new("CakeBox", _timeProvider.Now.AddHours(2), 1, Models.Cake.Chocolate, 120.25),
            new("ImportantCakeShop", _timeProvider.Now.AddHours(2), 1, Models.Cake.Vanilla, 120.25)
        };
    }

    public void UpdateOrders(Models.Order[] orders)
    {
    }
}