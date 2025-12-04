using System.ComponentModel.DataAnnotations;

namespace WebAPI.Requests;

/// <summary>
/// Represents a batch of order items.
/// </summary>
public sealed record OrdersRequest(
    [property: Required]
    [property: MinLength(1, ErrorMessage = "At least one order item is required.")]
    List<OrderItemRequest> Orders
);
