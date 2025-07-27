using System.ComponentModel.DataAnnotations;

namespace MetaExchange.Models;

public class OrderBook
{
    public DateTime AcqTime { get; set; }
    public List<OrderBookEntry> Bids { get; set; } = new();
    public List<OrderBookEntry> Asks { get; set; } = new();
}