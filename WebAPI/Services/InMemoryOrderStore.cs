using System.ComponentModel.DataAnnotations;
using WebAPI.Requests;

namespace WebAPI.Services;

/// <summary>
/// In-memory implementation of IOrderStore.
/// Uses a Dictionary with a lock to achieve correct snapshot + clear.
/// </summary>
public sealed class InMemoryOrderStore : IOrderStore
{
    private readonly Dictionary<int, int> _totals = new();
    private readonly object _lock = new();


    /// <inheritdoc />
    public void AddOrders(OrdersRequest ordersRequest)
    {
        // basic validations
        ArgumentNullException.ThrowIfNull(ordersRequest);
        ValidateOrdersRequest(ordersRequest);

        // Lock to guarantee that snapshot (GetAggregatedOrdersAndClear) sees a consistent state and that no orders are "lost" between snapshot and Clear().
        lock (_lock)
        {
            foreach (OrderItemRequest order in ordersRequest.OrderItems)
            {
                if (!_totals.TryGetValue(order.ProductId, out int current))
                {
                    _totals[order.ProductId] = order.Quantity;
                }
                else
                {
                    // overflow check
                    if (current > int.MaxValue - order.Quantity)
                    {
                        throw new OverflowException($"Quantity overflow for product {order.ProductId}. Previous total quantity was {current} and new increment was {order.Quantity}.");
                    }

                    _totals[order.ProductId] = current + order.Quantity;
                }
            }
        }
    }


    /// <inheritdoc />
    public Dictionary<int, int> GetAggregatedOrdersAndClear()
    {
        Dictionary<int, int> snapshot;

        lock (_lock)
        {
            // Copy current totals into a separate dictionary
            snapshot = new Dictionary<int, int>(_totals);

            // Clear internal state so that a new batch can be accumulated
            _totals.Clear();
        }

        return snapshot;
    }


    /// <summary>
    /// Model validation based on attributes on OrdersRequest and OrderItemRequest
    /// </summary>
    /// <param name="ordersRequest">Orders Request - wrapper of Order Item Requests</param>
    private static void ValidateOrdersRequest(OrdersRequest ordersRequest)
    {
        // Validate the OrdersRequest
        var ctxOrders = new ValidationContext(ordersRequest);
        Validator.ValidateObject(ordersRequest, ctxOrders, validateAllProperties: true);

        // Validate each OrderItemRequest
        foreach (OrderItemRequest orderItem in ordersRequest.OrderItems)
        {
            var ctxOrderItems = new ValidationContext(orderItem);
            Validator.ValidateObject(orderItem, ctxOrderItems, validateAllProperties: true);
        }
    }
}
