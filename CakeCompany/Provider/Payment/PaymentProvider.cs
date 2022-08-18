using CakeCompany.Models;

namespace CakeCompany.Provider.Payment;

internal class PaymentProvider : IPaymentProvider
{
    public PaymentIn Process(Models.Order order)
    {
        if (!string.IsNullOrWhiteSpace(order.ClientName) && order.ClientName.Contains("Important"))
        {
            return new PaymentIn
            {
                HasCreditLimit = false,
                IsSuccessful = true
            };
        }

        return new PaymentIn
        {
            HasCreditLimit = true,
            IsSuccessful = true
        };
    }
}