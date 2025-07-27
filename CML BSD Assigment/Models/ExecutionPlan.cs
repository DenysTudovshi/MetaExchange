namespace MetaExchange.Models;

public class ExecutionPlan
{
    public List<ExecutionOrder> Orders { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public decimal TotalCost { get; set; }
    public decimal AveragePrice { get; set; }
    public bool IsFullyExecuted { get; set; }
    public string Message { get; set; } = string.Empty;
}