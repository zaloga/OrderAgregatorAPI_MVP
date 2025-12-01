namespace WebAPI.Requests;

/// <summary>
/// Represents one item in an order request.
/// </summary>
public sealed record OrderItemRequest(
    int ProductId,
    int Quantity);
