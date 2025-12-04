using WebAPI.Requests;

namespace WebAPI.Services;

public interface IOrderStore
{
    /// <summary>
    /// Adds order items (its counts) into store keyed by ProductId.
    /// </summary>
    /// <param name="orders">Order items to aggregate.</param>
    void AddOrders(IEnumerable<OrderItemRequest> orders);

    /// <summary>
    /// Gets agregated order items (total counts).
    /// </summary>
    Dictionary<int, int> GetAgregatedOrdersAndClear();
}