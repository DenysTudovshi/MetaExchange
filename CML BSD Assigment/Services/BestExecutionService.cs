using MetaExchange.Models;

namespace MetaExchange.Services;
public class BestExecutionService : IBestExecutionService
{
    private readonly IOrderBookService _orderBookService;

    public BestExecutionService(IOrderBookService orderBookService)
    {
        _orderBookService = orderBookService;
    }

    public ExecutionPlan FindBestExecution(ExecutionRequest request)
    {
        if (request.OrderType == OrderType.Buy)
        {
            return FindBestBuyExecution(request);
        }
        else
        {
            return FindBestSellExecution(request);
        }
    }

    private ExecutionPlan FindBestBuyExecution(ExecutionRequest request)
    {
        var executionPlan = new ExecutionPlan();
        var allAsks = new List<(string ExchangeId, OrderBookEntry Ask, ExchangeBalance Balance)>();

        // Collect all available asks from all exchanges. Take up to amount limit
        foreach (var exchange in request.Exchanges)
        {
            var availableAsks = _orderBookService.GetAvailableAsks(exchange.OrderBook, request.Amount);
            foreach (var ask in availableAsks)
            {
                allAsks.Add((exchange.ExchangeId, ask, exchange.Balance));
            }
        }

        // Sort by price (lowest first for buying)
        allAsks = allAsks.OrderBy(x => x.Ask.Order.Price).ToList();

        decimal remainingAmount = request.Amount;
        decimal totalCost = 0;

        //simple algorithm - iterate through asks until desired amount is fulfilled and store Execution Order
        foreach (var (exchangeId, ask, balance) in allAsks)
        {
            if (remainingAmount <= 0) break;

            // Check if exchange has enough BTC to sell
            var maxAvailableFromBalance = balance.BTC;
            var maxFromOrder = ask.Order.Amount;
            var maxAvailable = Math.Min(maxFromOrder, maxAvailableFromBalance);

            if (maxAvailable <= 0) continue;

            var amountToTake = Math.Min(remainingAmount, maxAvailable);
            var cost = amountToTake * ask.Order.Price;

            executionPlan.Orders.Add(new ExecutionOrder
            {
                ExchangeId = exchangeId,
                Type = OrderType.Buy,
                Amount = amountToTake,
                Price = ask.Order.Price,
                Cost = cost
            });

            remainingAmount -= amountToTake;
            totalCost += cost;

            // Update balance for next iteration
            balance.BTC -= amountToTake;
        }

        executionPlan.TotalAmount = request.Amount - remainingAmount;
        executionPlan.TotalCost = totalCost;
        executionPlan.AveragePrice = executionPlan.TotalAmount > 0 ? totalCost / executionPlan.TotalAmount : 0;
        executionPlan.IsFullyExecuted = remainingAmount == 0;
        executionPlan.Message = remainingAmount > 0
            ? $"Partially executed. {remainingAmount:F8} BTC could not be fulfilled."
            : "Fully executed.";

        return executionPlan;
    }

    private ExecutionPlan FindBestSellExecution(ExecutionRequest request)
    {
        var executionPlan = new ExecutionPlan();
        var allBids = new List<(string ExchangeId, OrderBookEntry Bid, ExchangeBalance Balance)>();

        // Collect all available bids from all exchanges
        foreach (var exchange in request.Exchanges)
        {
            var availableBids = _orderBookService.GetAvailableBids(exchange.OrderBook, request.Amount);
            foreach (var bid in availableBids)
            {
                allBids.Add((exchange.ExchangeId, bid, exchange.Balance));
            }
        }

        // Sort by price (highest first for selling)
        allBids = allBids.OrderByDescending(x => x.Bid.Order.Price).ToList();

        decimal remainingAmount = request.Amount;
        decimal totalRevenue = 0;

        //simple algorithm - iterate through bids until desired amount is fulfilled and store Execution Order
        foreach (var (exchangeId, bid, balance) in allBids)
        {
            if (remainingAmount <= 0) break;

            // Check if exchange has enough EUR to buy
            var maxAffordableFromBalance = balance.EUR / bid.Order.Price;
            var maxFromOrder = bid.Order.Amount;
            var maxAvailable = Math.Min(maxFromOrder, maxAffordableFromBalance);

            if (maxAvailable <= 0) continue;

            var amountToTake = Math.Min(remainingAmount, maxAvailable);
            var revenue = amountToTake * bid.Order.Price;

            executionPlan.Orders.Add(new ExecutionOrder
            {
                ExchangeId = exchangeId,
                Type = OrderType.Sell,
                Amount = amountToTake,
                Price = bid.Order.Price,
                Cost = revenue
            });

            remainingAmount -= amountToTake;
            totalRevenue += revenue;

            // Update balance for next iteration
            balance.EUR -= revenue;
        }

        executionPlan.TotalAmount = request.Amount - remainingAmount;
        executionPlan.TotalCost = totalRevenue;
        executionPlan.AveragePrice = executionPlan.TotalAmount > 0 ? totalRevenue / executionPlan.TotalAmount : 0;
        executionPlan.IsFullyExecuted = remainingAmount == 0;
        executionPlan.Message = remainingAmount > 0
            ? $"Partially executed. {remainingAmount:F8} BTC could not be fulfilled."
            : "Fully executed.";

        return executionPlan;
    }
}