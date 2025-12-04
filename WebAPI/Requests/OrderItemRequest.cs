using System.ComponentModel.DataAnnotations;

namespace WebAPI.Requests;

/// <summary>
/// Represents one item in an order request.
/// </summary>
public sealed record OrderItemRequest(
    [property: Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    int ProductId,
    [property: Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    int Quantity);
