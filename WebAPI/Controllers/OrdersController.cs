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
    /// Accepts one or more valid order items and stores them for aggregation.
    /// Aggregated data are periodically flushed to internal system / console.
    /// </summary>
    /// <param name="request">Batch of incoming order items.</param>
    /// <returns>202 Accepted on success, 400 on validation error.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post([FromBody] OrdersRequest request)
    {

        var orders = request.Orders;

        _logger.LogInformation(
            "Received {Count} valid order items.",
            orders.Count);

        _orderStore.AddOrders(orders);

        // 202 Accepted on success
        return Accepted();
    }
}
