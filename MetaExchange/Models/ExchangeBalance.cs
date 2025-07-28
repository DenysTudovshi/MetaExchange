namespace MetaExchange.Models;

/// <summary>
/// Represents the available account balances on a specific cryptocurrency exchange.
/// These balances act as execution constraints in the meta-exchange system - 
/// we cannot spend more EUR than available or sell more BTC than we currently hold.
/// </summary>
public class ExchangeBalance
{
    /// <summary>
    /// The available Euro balance in our trading account on this exchange.
    /// This amount limits how much BTC we can purchase - we cannot buy BTC 
    /// that would cost more EUR than this balance allows.
    /// Must be non-negative. Value is in EUR currency units.
    /// </summary>
    public decimal EUR { get; set; }

    /// <summary>
    /// The available Bitcoin balance in our trading account on this exchange.
    /// This amount limits how much BTC we can sell - we cannot sell more BTC 
    /// than we currently hold in this balance.
    /// Must be non-negative. Value is in BTC cryptocurrency units.
    /// </summary>
    public decimal BTC { get; set; }
}