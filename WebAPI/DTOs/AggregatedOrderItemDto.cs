namespace WebAPI.DTOs;

/// <summary>
/// Represents aggregated order item.
/// </summary>
public sealed record AggregatedOrderItemDto(
    int ProductId,
    int TotalQuantity);
