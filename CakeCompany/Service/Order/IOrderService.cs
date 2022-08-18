using CakeCompany.Models;

namespace CakeCompany.Service.Order;

internal interface IOrderService
{
    IEnumerable<Product> Process(ICollection<Models.Order> orders);
}