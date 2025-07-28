namespace MetaExchange.Models;

/// <summary>
/// Represents a single entry in the order book's bid or ask collection.
/// Acts as a wrapper around individual market orders, providing a consistent structure
/// for order book data and allowing for future extensibility of order book entries
/// without modifying the core Order class.
/// </summary>
public class OrderBookEntry
{
    /// <summary>
    /// The underlying market order containing the trading details (price, amount, type, etc.).
    /// This order represents an actual trading opportunity from another market participant
    /// that our meta-exchange algorithm can potentially execute against to fulfill client requests.
    /// For bid entries: An order from someone wanting to buy BTC (we can sell to them).
    /// For ask entries: An order from someone wanting to sell BTC (we can buy from them).
    /// </summary>
    public Order Order { get; set; } = new();
}