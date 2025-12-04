using WebAPI.Requests;
using WebAPI.Services;

namespace WebAPI.Tests;

public class OrderStoreUnitTests
{
    [Fact]
    public void FirstAndSecondSnapshot_OrderStore_Test()
    {
        // Arrange
        var orderStore = new InMemoryOrderStore();
        orderStore.AddOrders(
            new OrdersRequest()
            {
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 123456, Quantity = 10 },
                    new OrderItemRequest { ProductId = 456789, Quantity = 20 }
                }
            }
        );
        orderStore.AddOrders(
            new OrdersRequest()
            {
                OrderItems = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 123456, Quantity = 30 },
                    new OrderItemRequest { ProductId = 456789, Quantity = 40 }
                }
            }
        );

        // Act
        Dictionary<int, int> firstStoreSnapshot = orderStore.GetAggregatedOrdersAndClear();
        Dictionary<int, int> secondStoreSnapshot = orderStore.GetAggregatedOrdersAndClear();

        // Assert
        Assert.Equal(2, firstStoreSnapshot.Count); // expects 2 different products in first order store snapshot
        Assert.Equal(40, firstStoreSnapshot[123456]); // expects 40 pieces of product 123456
        Assert.Equal(60, firstStoreSnapshot[456789]); // expects 60 pieces of product 456789
        Assert.Empty(secondStoreSnapshot); // expects 0 different products in second order store snapshot
    }


    [Fact]
    public void AggregateQuantities_OrderStore_Test()
    {
        // Arrange
        var orderStore = new InMemoryOrderStore();
        var ordersCollection = new List<OrderItemRequest>();
        for (int i = 0; i < 100; i++) // prepare 100x 4 different procuts and its pieces in orders collection
        {
            ordersCollection.AddRange(new[]
            {
                new OrderItemRequest { ProductId = 123456, Quantity = 11 },
                new OrderItemRequest { ProductId = 234567, Quantity = 22 },
                new OrderItemRequest { ProductId = 345678, Quantity = 33 },
                new OrderItemRequest { ProductId = 456789, Quantity = 44 }
            });
        }

        // Act
        for (int i = 0; i < 100; i++) // 100x add the prepared orders collection into order store
        {
            orderStore.AddOrders(new OrdersRequest() { OrderItems = ordersCollection });
        }
        Dictionary<int, int> storeSnapshot = orderStore.GetAggregatedOrdersAndClear();

        // Assert
        Assert.Equal(4, storeSnapshot.Count); //expects 4 different products in first order store snapshot
        Assert.Equal(110000, storeSnapshot[123456]); // expects 110000 pieces of product 123456
        Assert.Equal(220000, storeSnapshot[234567]); // expects 220000 pieces of product 234567
        Assert.Equal(330000, storeSnapshot[345678]); // expects 330000 pieces of product 345678
        Assert.Equal(440000, storeSnapshot[456789]); // expects 440000 pieces of product 456789
    }
}
