namespace MetaExchange.Models;

public class ExecutionOrder
{
    public string ExchangeId { get; set; } = string.Empty;
    public OrderType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}