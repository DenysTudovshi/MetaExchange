using MetaExchange.Models;

namespace MetaExchange.Services;
public class OrderBookService : IOrderBookService
{
    /// <inheritdoc />
    public List<OrderBookEntry> GetAvailableAsks(OrderBook orderBook, decimal maxAmount)
    {
        var availableAsks = new List<OrderBookEntry>();
        decimal totalAmount = 0;

        foreach (var ask in orderBook.Asks.OrderBy(a => a.Order.Price))
        {
            if (totalAmount >= maxAmount) break;

            var remainingNeeded = maxAmount - totalAmount;
            var availableAmount = Math.Min(ask.Order.Amount, remainingNeeded);

            if (availableAmount > 0)
            {
                availableAsks.Add(new OrderBookEntry
                {
                    Order = new Order
                    {
                        Type = ask.Order.Type,
                        Kind = ask.Order.Kind,
                        Amount = availableAmount,
                        Price = ask.Order.Price
                    }
                });
                totalAmount += availableAmount;
            }
        }

        return availableAsks;
    }

    /// <inheritdoc />
    public List<OrderBookEntry> GetAvailableBids(OrderBook orderBook, decimal maxAmount)
    {
        var availableBids = new List<OrderBookEntry>();
        decimal totalAmount = 0;

        foreach (var bid in orderBook.Bids.OrderByDescending(b => b.Order.Price))
        {
            if (totalAmount >= maxAmount) break;

            var remainingNeeded = maxAmount - totalAmount;
            var availableAmount = Math.Min(bid.Order.Amount, remainingNeeded);

            if (availableAmount > 0)
            {
                availableBids.Add(new OrderBookEntry
                {
                    Order = new Order
                    {
                        Type = bid.Order.Type,
                        Kind = bid.Order.Kind,
                        Amount = availableAmount,
                        Price = bid.Order.Price
                    }
                });
                totalAmount += availableAmount;
            }
        }

        return availableBids;
    }
}

