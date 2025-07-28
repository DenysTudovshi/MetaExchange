namespace MetaExchange.Models;

/// <summary>
/// Represents a cryptocurrency exchange with its associated account balance constraints and market data.
/// Used in the meta-exchange system to aggregate trading opportunities across multiple exchanges
/// while respecting the available funds on each platform.
/// </summary>
public class Exchange
{
    /// <summary>
    /// Unique identifier for the cryptocurrency exchange platform.
    /// Examples: "Binance", "Coinbase", "Kraken", "Exchange-1"
    /// Used to track which exchange each trade execution should be routed to.
    /// </summary>
    public string ExchangeId { get; set; } = string.Empty;

    /// <summary>
    /// The available EUR and BTC balances in our trading account on this exchange.
    /// These balances act as constraints for trade execution - we cannot buy more BTC 
    /// than our EUR balance allows, or sell more BTC than we currently hold.
    /// </summary>
    public ExchangeBalance Balance { get; set; } = new();

    /// <summary>
    /// Current market data from this exchange, containing available buy orders (bids) 
    /// and sell orders (asks) from other traders. This data is used to determine
    /// the best available prices for trade execution on this specific exchange.
    /// </summary>
    public OrderBook OrderBook { get; set; } = new();
}
