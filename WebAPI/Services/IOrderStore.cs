using WebAPI.Requests;

namespace WebAPI.Services;

public interface IOrderStore
{
    void AddOrders(IEnumerable<OrderItemRequest> orders);
    Dictionary<int, int> GetAgregatedOrdersAndClear();
}