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


    /// <summary>
    /// Adds order items (its counts) into in-memmory collection (Dictionary) keyed by ProductId.
    /// </summary>
    public void AddOrders(IEnumerable<OrderItemRequest> orders)
    {
        if (orders is null)
        {
            throw new ArgumentNullException(nameof(orders));
        }

        // Lock to guarantee that snapshot (GetAgregatedOrdersAndClear) sees a consistent state and that no orders are "lost" between snapshot and Clear().
        lock (_lock)
        {
            foreach (OrderItemRequest order in orders)
            {
                if (order.ProductId < 0)
                {
                    // skip invalid product IDs.
                    continue;
                }

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


    /// <summary>
    /// Gets agregated order items (total counts).
    /// </summary>
    public Dictionary<int, int> GetAgregatedOrdersAndClear()
    {
        Dictionary<int, int> snapshot;

        lock (_lock)
        {
            // Copy current totals into a separate dictionary
            snapshot = new Dictionary<int, int>(_totals);

            // Clear internal state so that new batch can be accumulated
            _totals.Clear();
        }

        return snapshot;
    }
}
