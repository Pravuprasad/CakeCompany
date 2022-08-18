namespace CakeCompany.Models.Transport;

internal interface ITransport
{
    bool Deliver(List<Product> products);
}