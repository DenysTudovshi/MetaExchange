using MetaExchange.Models;

namespace CML_BSD_Assigment.Models
{
    public class Exchange
    {
        public string ExchangeId { get; set; } = string.Empty;
        public ExchangeBalance Balance { get; set; } = new();
        public OrderBook OrderBook { get; set; } = new();
    }
}
