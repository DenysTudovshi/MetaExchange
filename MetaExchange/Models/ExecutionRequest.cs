namespace MetaExchange.Models;

/// <summary>
/// Represents a request to find the best execution plan for buying or selling Bitcoin 
/// across multiple cryptocurrency exchanges. Contains the trade specifications and 
/// available exchange data needed by the meta-exchange algorithm to optimize order routing.
/// </summary>
public class ExecutionRequest
{
    /// <summary>
    /// The type of trade operation to perform - either Buy (purchase BTC with EUR) or Sell (sell BTC for EUR).
    /// Determines whether the algorithm will search for the lowest ask prices (for buying) 
    /// or highest bid prices (for selling) to optimize the execution outcome.
    /// </summary>
    public OrderType OrderType { get; set; }

    /// <summary>
    /// The total amount of Bitcoin to be traded, expressed in BTC units.
    /// Must be positive. The meta-exchange algorithm will attempt to fulfill this entire amount
    /// by finding the optimal combination of orders across the provided exchanges,
    /// subject to available liquidity and balance constraints.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The collection of cryptocurrency exchanges to consider for trade execution.
    /// Each exchange includes its current market data (order book) and our available account balances.
    /// The algorithm will analyze all provided exchanges to find the optimal price execution
    /// while respecting the balance constraints on each platform.
    /// </summary>
    public List<Exchange> Exchanges { get; set; } = new();
}