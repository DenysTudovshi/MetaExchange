namespace MetaExchange.Models;

/// <summary>
/// Represents a snapshot of market data from a cryptocurrency exchange at a specific point in time.
/// Contains the current buy and sell orders available for Bitcoin/EUR trading,
/// which the meta-exchange algorithm analyzes to find optimal execution prices and liquidity.
/// </summary>
public class OrderBook
{
    /// <summary>
    /// The timestamp when this order book snapshot was acquired from the exchange.
    /// Used to ensure market data freshness and for temporal analysis.
    /// Critical for high-frequency trading where order book data can change rapidly.
    /// </summary>
    public DateTime AcqTime { get; set; }

    /// <summary>
    /// Collection of buy orders (bids) from other traders wanting to purchase Bitcoin with EUR.
    /// Sorted by price in descending order (highest bids first) in a typical order book.
    /// These represent potential selling opportunities for our meta-exchange when we want to sell BTC.
    /// Each bid shows the maximum price a buyer is willing to pay and the quantity they want.
    /// </summary>
    public List<OrderBookEntry> Bids { get; set; } = new();

    /// <summary>
    /// Collection of sell orders (asks) from other traders wanting to sell Bitcoin for EUR.
    /// Sorted by price in ascending order (lowest asks first) in a typical order book.
    /// These represent potential buying opportunities for our meta-exchange when we want to buy BTC.
    /// Each ask shows the minimum price a seller will accept and the quantity they're offering.
    /// </summary>
    public List<OrderBookEntry> Asks { get; set; } = new();
}