namespace MetaExchange.Services;

using MetaExchange.Models;

public interface IOrderBookService
{
    List<OrderBookEntry> GetAvailableAsks(OrderBook orderBook, decimal maxAmount);
    List<OrderBookEntry> GetAvailableBids(OrderBook orderBook, decimal maxAmount);
}
