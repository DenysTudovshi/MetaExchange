using Xunit;
using MetaExchange.Services;
using MetaExchange.Models;
using Moq;
using CML_BSD_Assigment.Models;

namespace MetaExchange.Tests;
public class BestExecutionServiceTests
{
    private readonly Mock<IOrderBookService> _mockOrderBookService;
    private readonly BestExecutionService _bestExecutionService;

    public BestExecutionServiceTests()
    {
        _mockOrderBookService = new Mock<IOrderBookService>();
        _bestExecutionService = new BestExecutionService(_mockOrderBookService.Object);
    }

    [Fact]
    public void FindBestExecution_BuyOrder_ReturnsLowestPriceFirst()
    {
        // Arrange
        var exchange1 = new Exchange
        {
            ExchangeId = "Exchange1",
            Balance = new ExchangeBalance { EUR = 10000, BTC = 5 },
            OrderBook = new OrderBook()
        };

        var exchange2 = new Exchange
        {
            ExchangeId = "Exchange2",
            Balance = new ExchangeBalance { EUR = 10000, BTC = 5 },
            OrderBook = new OrderBook()
        };

        var asks1 = new List<OrderBookEntry>
            {
                new() { Order = new Order { Amount = 1m, Price = 3000m } }
            };

        var asks2 = new List<OrderBookEntry>
            {
                new() { Order = new Order { Amount = 1m, Price = 2900m } }
            };

        _mockOrderBookService.Setup(x => x.GetAvailableAsks(exchange1.OrderBook, It.IsAny<decimal>()))
            .Returns(asks1);
        _mockOrderBookService.Setup(x => x.GetAvailableAsks(exchange2.OrderBook, It.IsAny<decimal>()))
            .Returns(asks2);

        var request = new ExecutionRequest
        {
            OrderType = OrderType.Buy,
            Amount = 1.5m,
            Exchanges = new List<Exchange> { exchange1, exchange2 }
        };

        // Act
        var result = _bestExecutionService.FindBestExecution(request);

        // Assert
        Assert.True(result.IsFullyExecuted);
        Assert.Equal(2, result.Orders.Count);
        Assert.Equal(2900m, result.Orders[0].Price); // Should get cheaper exchange first
        Assert.Equal(3000m, result.Orders[1].Price);
    }

    [Fact]
    public void FindBestExecution_SellOrder_ReturnsHighestPriceFirst()
    {
        // Arrange
        var exchange1 = new Exchange
        {
            ExchangeId = "Exchange1",
            Balance = new ExchangeBalance { EUR = 10000, BTC = 5 },
            OrderBook = new OrderBook()
        };

        var exchange2 = new Exchange
        {
            ExchangeId = "Exchange2",
            Balance = new ExchangeBalance { EUR = 10000, BTC = 5 },
            OrderBook = new OrderBook()
        };

        var bids1 = new List<OrderBookEntry>
            {
                new() { Order = new Order { Amount = 1m, Price = 3000m } }
            };

        var bids2 = new List<OrderBookEntry>
            {
                new() { Order = new Order { Amount = 1m, Price = 3100m } }
            };

        _mockOrderBookService.Setup(x => x.GetAvailableBids(exchange1.OrderBook, It.IsAny<decimal>()))
            .Returns(bids1);
        _mockOrderBookService.Setup(x => x.GetAvailableBids(exchange2.OrderBook, It.IsAny<decimal>()))
            .Returns(bids2);

        var request = new ExecutionRequest
        {
            OrderType = OrderType.Sell,
            Amount = 1.5m,
            Exchanges = new List<Exchange> { exchange1, exchange2 }
        };

        // Act
        var result = _bestExecutionService.FindBestExecution(request);

        // Assert
        Assert.True(result.IsFullyExecuted);
        Assert.Equal(2, result.Orders.Count);
        Assert.Equal(3100m, result.Orders[0].Price); // Should get higher price first
        Assert.Equal(3000m, result.Orders[1].Price);
    }
}
