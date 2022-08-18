using CakeCompany.Models;

namespace CakeCompany.Provider.Payment;

internal interface IPaymentProvider
{
    PaymentIn Process(Models.Order order);
}