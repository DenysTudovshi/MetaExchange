using MetaExchange.Models;

namespace MetaExchange.Services;

/// <summary>
/// Defines the contract for processing order book data and extracting available trading opportunities.
/// Responsible for filtering and preparing market orders from exchange order books
/// based on liquidity requirements and trading constraints for the meta-exchange algorithm.
/// </summary>
public interface IOrderBookService
{
    /// <summary>
    /// Extracts available sell orders (asks) from an order book that can be used for buying Bitcoin.
    /// Filters and sorts the ask orders to identify viable purchase opportunities,
    /// typically ordering by price (lowest first) to optimize buying costs.
    /// </summary>
    /// <param name="orderBook">
    /// The order book containing market data from a specific exchange.
    /// Must contain ask orders (sell orders) in the Asks collection.
    /// </param>
    /// <param name="maxAmount">
    /// The maximum amount of Bitcoin to consider across all ask orders, expressed in BTC units.
    /// Used to limit the scope of orders returned and optimize processing for large order books.
    /// </param>
    /// <returns>
    /// A filtered and processed collection of ask orders that represent viable buying opportunities.
    /// Orders are typically sorted by price (ascending) to prioritize the most favorable execution prices.
    /// May contain fewer orders than requested if insufficient liquidity is available.
    /// </returns>
    List<OrderBookEntry> GetAvailableAsks(OrderBook orderBook, decimal maxAmount);

    /// <summary>
    /// Extracts available buy orders (bids) from an order book that can be used for selling Bitcoin.
    /// Filters and sorts the bid orders to identify viable selling opportunities,
    /// typically ordering by price (highest first) to optimize selling revenue.
    /// </summary>
    /// <param name="orderBook">
    /// The order book containing market data from a specific exchange.
    /// Must contain bid orders (buy orders) in the Bids collection.
    /// </param>
    /// <param name="maxAmount">
    /// The maximum amount of Bitcoin to consider across all bid orders, expressed in BTC units.
    /// Used to limit the scope of orders returned and optimize processing for large order books.
    /// </param>
    /// <returns>
    /// A filtered and processed collection of bid orders that represent viable selling opportunities.
    /// Orders are typically sorted by price (descending) to prioritize the most favorable execution prices.
    /// May contain fewer orders than requested if insufficient liquidity is available.
    /// </returns>
    List<OrderBookEntry> GetAvailableBids(OrderBook orderBook, decimal maxAmount);
}
