using MetaExchange.Models;

namespace MetaExchange.Services;
public class BestExecutionService : IBestExecutionService
{
    private readonly IOrderBookService _orderBookService;

    public BestExecutionService(IOrderBookService orderBookService)
    {
        _orderBookService = orderBookService;
    }

    /// <inheritdoc />
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

    /// <summary>
    /// Finds the optimal execution plan for buying Bitcoin by aggregating the lowest-priced sell orders
    /// across all provided exchanges. Implements a straightforward sorting-based approach for simplicity
    /// and code readability, though more efficient algorithms could be applied for high-frequency scenarios.
    /// 
    /// Algorithm: Collects all available asks from exchanges, sorts by price ascending, then fills
    /// the requested amount starting with the cheapest orders. This O(n log n) approach was chosen
    /// over more efficient alternatives like a min-heap/priority queue (O(n log k)) or merge algorithm
    /// for pre-sorted order books (O(n)) to maintain clear, maintainable code flow.
    /// 
    /// The method applies exchange balance constraints to ensure orders don't exceed available BTC
    /// on each exchange, and tracks remaining balances to prevent over-allocation during execution planning.
    /// </summary>
    /// <param name="request">The buy execution request containing amount and exchange data</param>
    /// <returns>
    /// An execution plan with orders sorted by ascending price (cheapest first),
    /// minimizing the total cost for the requested Bitcoin purchase amount
    /// </returns>
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

        //Form clear report response with result description in case plan was only partially executed.
        executionPlan.TotalAmount = request.Amount - remainingAmount;
        executionPlan.TotalCost = totalCost;
        executionPlan.AveragePrice = executionPlan.TotalAmount > 0 ? totalCost / executionPlan.TotalAmount : 0;
        executionPlan.IsFullyExecuted = remainingAmount == 0;
        executionPlan.Message = remainingAmount > 0
            ? $"Partially executed. {remainingAmount:F8} BTC could not be fulfilled."
            : "Fully executed.";

        return executionPlan;
    }

    /// <summary>
    /// Finds the optimal execution plan for selling Bitcoin by aggregating the highest-priced buy orders
    /// across all provided exchanges. Implements a straightforward sorting-based approach for simplicity
    /// and code readability, though more efficient algorithms could be applied for high-frequency scenarios.
    /// 
    /// Algorithm: Collects all available bids from exchanges, sorts by price descending, then fills
    /// the requested amount starting with the highest-paying orders. This O(n log n) approach was chosen
    /// over more efficient alternatives like a max-heap/priority queue (O(n log k)) or merge algorithm
    /// for pre-sorted order books (O(n)) to maintain clear, maintainable code flow.
    /// 
    /// The method applies exchange balance constraints to ensure orders don't exceed available EUR
    /// on each exchange, and tracks remaining balances to prevent over-allocation during execution planning.
    /// </summary>
    /// <param name="request">The sell execution request containing amount and exchange data</param>
    /// <returns>
    /// An execution plan with orders sorted by descending price (highest paying first),
    /// maximizing the total revenue for the requested Bitcoin sale amount
    /// </returns>
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

        //Form clear report response with result description in case plan was only partially executed.
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