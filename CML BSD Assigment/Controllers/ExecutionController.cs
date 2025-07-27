namespace MetaExchange.Controllers
{
    using CML_BSD_Assigment.Models;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("api/[controller]")]
    public class ExecutionController : ControllerBase
    {
        private readonly IBestExecutionService _bestExecutionService;
        private readonly ILogger<ExecutionController> _logger;

        public ExecutionController(IBestExecutionService bestExecutionService, ILogger<ExecutionController> logger)
        {
            _bestExecutionService = bestExecutionService;
            _logger = logger;
        }

        /// <summary>
        /// Find the best execution plan for buying or selling BTC across multiple exchanges
        /// </summary>
        /// <param name="request">Execution request containing order type, amount, and order books</param>
        /// <returns>Best execution plan</returns>
        [HttpPost("best-execution")]
        [ProducesResponseType(typeof(ApiResponse<ExecutionPlan>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public async Task<ActionResult<ApiResponse<ExecutionPlan>>> GetBestExecution([FromBody] ExecutionRequest request)
        {
            try
            {
                _logger.LogInformation("Processing best execution request for {OrderType} {Amount} BTC",
                    request.OrderType, request.Amount);

                var validationResult = ValidateRequest(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors
                    });
                }

                var executionPlan = _bestExecutionService.FindBestExecution(request);

                return Ok(new ApiResponse<ExecutionPlan>
                {
                    Success = true,
                    Data = executionPlan,
                    Message = "Best execution plan calculated successfully"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument in execution request");
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request parameters",
                    Errors = new List<string> { ex.Message }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing best execution request");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An internal error occurred while processing the request",
                    Errors = new List<string> { "Please try again later or contact support" }
                });
            }
        }

        private ValidationResult ValidateRequest(ExecutionRequest request)
        {
            var errors = new List<string>();

            if (request.Amount <= 0)
            {
                errors.Add("Amount must be greater than zero");
            }

            if (request.Exchanges == null || !request.Exchanges.Any())
            {
                errors.Add("At least one exchange must be provided");
            }
            else
            {
                for (int i = 0; i < request.Exchanges.Count; i++)
                {
                    var exchange = request.Exchanges[i];

                    if (string.IsNullOrWhiteSpace(exchange.ExchangeId))
                    {
                        errors.Add($"Exchange[{i}]: ExchangeId is required");
                    }

                    if (exchange.Balance.EUR < 0)
                    {
                        errors.Add($"Exchange[{i}]: EUR balance cannot be negative");
                    }

                    if (exchange.Balance.BTC < 0)
                    {
                        errors.Add($"Exchange[{i}]: BTC balance cannot be negative");
                    }

                    // Validate that we have either bids or asks
                    if ((exchange.OrderBook.Bids == null || !exchange.OrderBook.Bids.Any()) &&
                        (exchange.OrderBook.Asks == null || !exchange.OrderBook.Asks.Any()))
                    {
                        errors.Add($"Exchange[{i}]: Must have at least one bid or ask order");
                    }
                }
            }

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}