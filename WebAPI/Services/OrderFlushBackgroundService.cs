using System.Text.Json;
using WebAPI.DTOs;

namespace WebAPI.Services;

/// <summary>
/// Periodically takes aggregated order items from IOrderStore and sends them to console output in specified interval.
/// </summary>
public sealed class OrderFlushBackgroundService(
    ILogger<OrderFlushBackgroundService> logger,
    IOrderStore orderStore,
    IConfiguration configuration
    ) : BackgroundService
{
    private readonly ILogger<OrderFlushBackgroundService> _logger = logger;
    private readonly IOrderStore _orderStore = orderStore;
    private readonly int _flushIntervalSeconds = configuration.GetValue<int>(
            "AggregatorFlushConfiguration:FlushIntervalSeconds");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order flush background service starting with interval {Interval} seconds.", _flushIntervalSeconds);

        // Simple timer loop: delay -> flush -> repeat
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Wait for the configured interval.
                TimeSpan interval = TimeSpan.FromSeconds(_flushIntervalSeconds);
                await Task.Delay(interval, stoppingToken);

                // get current state (snapshot) and clear all
                Dictionary<int, int> storeSnapshot = _orderStore.GetAgregatedOrdersAndClear();
                if (storeSnapshot.Count == 0)
                {
                    // Nothing to send this time – skip
                    continue;
                }

                // Convert the dictionary to DTOs to improve readability of the output
                List<AggregatedOrderItemDto> aggregatedOrderItems = storeSnapshot
                    .Select(kvp => new AggregatedOrderItemDto(kvp.Key, kvp.Value))
                    .ToList();

                string jsonOutput = JsonSerializer.Serialize(aggregatedOrderItems);

                _logger.LogInformation("Flushing {Count} aggregated order items to internal system.", aggregatedOrderItems.Count);

                Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Aggregated orders: {jsonOutput}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while flushing aggregated orders.");
            }
        }

        _logger.LogInformation("Order flush background service is stopped.");
    }
}
