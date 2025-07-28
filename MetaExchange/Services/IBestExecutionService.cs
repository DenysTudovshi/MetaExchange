using MetaExchange.Models;

namespace MetaExchange.Services;

/// <summary>
/// Defines the contract for the meta-exchange best execution algorithm service.
/// Responsible for analyzing multiple cryptocurrency exchanges to find the optimal
/// combination of trades that achieves the best possible price for buying or selling Bitcoin,
/// while respecting exchange balance constraints and available market liquidity.
/// </summary>
public interface IBestExecutionService
{
    /// <summary>
    /// Analyzes the provided exchanges and their order books to determine the optimal execution plan
    /// for the requested Bitcoin trade. The algorithm aggregates liquidity across multiple exchanges,
    /// sorts opportunities by price (lowest asks for buying, highest bids for selling),
    /// and creates a sequence of trades that minimizes cost for purchases or maximizes revenue for sales.
    /// </summary>
    /// <param name="request">
    /// The execution request containing the trade specifications (buy/sell, amount) 
    /// and available exchange data (order books and balance constraints).
    /// Must include at least one exchange with valid market data.
    /// </param>
    /// <returns>
    /// A comprehensive execution plan detailing the optimal sequence of trades across exchanges,
    /// including individual order specifications, total costs, average pricing, and execution status.
    /// Returns partial execution plans when insufficient liquidity or balance constraints prevent full execution.
    /// </returns>
    ExecutionPlan FindBestExecution(ExecutionRequest request);
}