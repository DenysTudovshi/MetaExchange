using CML_BSD_Assigment.Models;

namespace MetaExchange.Models;

public class ExecutionRequest
{
    public OrderType OrderType { get; set; }
    public decimal Amount { get; set; }
    public List<Exchange> Exchanges { get; set; } = new();
}