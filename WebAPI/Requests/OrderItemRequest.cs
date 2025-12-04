using System.ComponentModel.DataAnnotations;

namespace WebAPI.Requests;

/// <summary>
/// Represents one item in an order request.
/// </summary>
public sealed class OrderItemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    public int ProductId { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; init; }
}