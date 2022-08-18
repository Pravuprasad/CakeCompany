namespace CakeCompany.Provider.Order;

internal interface IOrderProvider
{
    Models.Order[] GetLatestOrders();

    void UpdateOrders(Models.Order[] orders);
}