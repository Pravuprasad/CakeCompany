using CakeCompany.Models;

namespace CakeCompany.Provider.Transport;

internal interface ITransportProvider
{
    string CheckForAvailability(List<Product> products);
}