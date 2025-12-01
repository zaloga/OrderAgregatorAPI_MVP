using Microsoft.AspNetCore.Mvc;
using WebAPI.Requests;
using WebAPI.Services;

namespace WebAPI.Controllers;

/// <summary>
/// API controller for submitting order items to be aggregated.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrderStore _orderStore;


    public OrdersController(ILogger<OrdersController> logger, IOrderStore orderStore)
    {
        _logger = logger;
        _orderStore = orderStore;
    }


    /// <summary>
    /// Accepts one or more order items and stores them for aggregation.
    /// Aggregated data are periodically flushed to internal system / console.
    /// </summary>
    /// <param name="orders">Array of incoming order items.</param>
    /// <returns>202 Accepted on success, 400 on validation error.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post(
        [FromBody] List<OrderItemRequest> orders)
    {
        // Basic request validation
        if (orders is null || orders.Count == 0)
        {
            string error = "Request body must contain at least one order item.";

            _logger.LogWarning(error);

            return BadRequest(new
            {
                Error = error
            });
        }

        // Filter out invalid items
        var validOrders = orders
            .Where(o => o.ProductId > 0 && o.Quantity > 0)
            .ToList();

        if (validOrders.Count == 0)
        {
            string error = "All order items have invalid productId or quantity.";

            _logger.LogWarning(error);

            return BadRequest(new
            {
                Error = error
            });
        }

        _logger.LogInformation(
            "Received {Count} order items and from that {ValidCount} valid order items.",
            orders.Count,
            validOrders.Count);

        _orderStore.AddOrders(validOrders);

        // 202 Accepted on success
        return Accepted();
    }
}
