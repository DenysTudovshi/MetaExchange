namespace MetaExchange.Models;

/// <summary>
/// Represents a trading order from the cryptocurrency exchange's order book.
/// Contains market data from other traders who have placed buy or sell orders,
/// which the meta-exchange algorithm analyzes to find optimal execution opportunities.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for this order on the exchange platform.
    /// May be null for aggregated or anonymized order book data.
    /// Used for order tracking and reference purposes.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The timestamp when this order was placed on the exchange.
    /// Used for order book data freshness validation and temporal analysis.
    /// May be default DateTime for static or test data.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Indicates whether this is a buy order (trader wants to purchase BTC with EUR) 
    /// or a sell order (trader wants to sell BTC for EUR).
    /// Determines which side of the order book this order appears on.
    /// </summary>
    public OrderType Type { get; set; }

    /// <summary>
    /// The execution type of this order - either Limit (execute only at specified price or better)
    /// or Market (execute immediately at best available price).
    /// Most order book data consists of Limit orders showing specific price levels.
    /// </summary>
    public OrderKind Kind { get; set; }

    /// <summary>
    /// The quantity of Bitcoin available in this order, expressed in BTC units.
    /// Represents the maximum amount that can be traded against this order
    /// before it is fully consumed from the order book.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The price per Bitcoin for this order, expressed in EUR per BTC.
    /// For Buy orders: The maximum price the trader is willing to pay.
    /// For Sell orders: The minimum price the trader is willing to accept.
    /// </summary>
    public decimal Price { get; set; }
}

/// <summary>
/// Defines the direction of a trading order from the trader's perspective.
/// </summary>
public enum OrderType
{
    /// <summary>
    /// A buy order where the trader wants to purchase Bitcoin using EUR.
    /// These orders appear on the "bid" side of the order book.
    /// </summary>
    Buy,

    /// <summary>
    /// A sell order where the trader wants to sell Bitcoin for EUR.
    /// These orders appear on the "ask" side of the order book.
    /// </summary>
    Sell
}

/// <summary>
/// Defines the execution behavior and pricing strategy of a trading order.
/// </summary>
public enum OrderKind
{
    /// <summary>
    /// A limit order that will only execute at the specified price or better.
    /// Provides price certainty but no guarantee of execution.
    /// Most common type in order book market data.
    /// </summary>
    Limit,

    /// <summary>
    /// A market order that executes immediately at the best available price.
    /// Provides execution certainty but no price guarantee.
    /// Less common in static order book snapshots.
    /// </summary>
    Market
}
