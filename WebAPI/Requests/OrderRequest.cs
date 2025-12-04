using System.ComponentModel.DataAnnotations;

namespace WebAPI.Requests;

/// <summary>
/// Represents a batch of order items.
/// </summary>
public sealed record OrdersRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<OrderItemRequest> OrderItems { get; init; } = [];
}