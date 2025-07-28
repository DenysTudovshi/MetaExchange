namespace MetaExchange.Models;

/// <summary>
/// Represents a specific trade order that should be executed on a particular exchange
/// as part of the optimal execution plan. Contains all the details needed by the trading system
/// to route and execute the order, including exchange destination, order specifications, and cost calculations.
/// </summary>
public class ExecutionOrder
{
    /// <summary>
    /// The identifier of the cryptocurrency exchange where this order should be executed.
    /// Examples: "Binance", "Coinbase", "Kraken", "Exchange-1"
    /// Used by the trading system to route the order to the correct exchange platform.
    /// </summary>
    public string ExchangeId { get; set; } = string.Empty;

    /// <summary>
    /// The type of order to be executed - either Buy (purchase BTC with EUR) or Sell (sell BTC for EUR).
    /// Determines the direction of the trade and which side of the order book will be used.
    /// </summary>
    public OrderType Type { get; set; }

    /// <summary>
    /// The quantity of Bitcoin to be traded in this order, expressed in BTC units.
    /// Must be positive and cannot exceed the available liquidity at the specified price
    /// or the exchange's balance constraints.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The price per Bitcoin at which this order should be executed, expressed in EUR per BTC.
    /// This represents the optimal price found by the best execution algorithm
    /// from the available market liquidity on the target exchange.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The total cost or revenue of this order in EUR currency.
    /// For Buy orders: Total EUR cost (Amount × Price)
    /// For Sell orders: Total EUR revenue (Amount × Price)
    /// Used for cost analysis and settlement calculations.
    /// </summary>
    public decimal Cost { get; set; }
}