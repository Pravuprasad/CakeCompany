using CakeCompany.Models;

namespace CakeCompany.Provider.Cake;

internal interface ICakeProvider
{
    DateTime Check(Models.Order order);

    Product Bake(Models.Order order);
}