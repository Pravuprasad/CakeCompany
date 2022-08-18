using CakeCompany.Models;

namespace CakeCompany.Service.Transport;

internal interface ITransportService
{
    void Deliver(List<Product> products);
}