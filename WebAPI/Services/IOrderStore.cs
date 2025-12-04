using WebAPI.Requests;

namespace WebAPI.Services;

public interface IOrderStore
{
    /// <summary>
    /// Adds order items (its counts) into store keyed by ProductId.
    /// </summary>
    /// <param name="ordersRequest">Order items to aggregate.</param>
    void AddOrders(OrdersRequest ordersRequest);

    /// <summary>
    /// Gets aggregated order items (total counts).
    /// </summary>
    Dictionary<int, int> GetAggregatedOrdersAndClear();
}