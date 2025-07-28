using FluentValidation;
using MetaExchange.Models;
using MetaExchange.Services;
using MetaExchange.Validators;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchange.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecutionController : ControllerBase
{
    private readonly IBestExecutionService _bestExecutionService;
    private readonly ILogger<ExecutionController> _logger;
    private readonly IValidator<ExecutionRequest> _validator;

    public ExecutionController(IBestExecutionService bestExecutionService,
        ILogger<ExecutionController> logger,
        IValidator<ExecutionRequest> validator)
    {
        _bestExecutionService = bestExecutionService;
        _logger = logger;
        _validator = validator;
    }

    /// <summary>
    /// Find the best execution plan for buying or selling BTC across multiple exchanges
    /// </summary>
    /// <param name="request">Execution request containing order type, amount, and order books</param>
    /// <returns>Best execution plan</returns>
    [HttpPost("best-execution")]
    [ProducesResponseType(typeof(ApiResponse<ExecutionPlan>), 200)]
    [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), 400)]
    [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), 500)]
    public async Task<ActionResult<ApiResponse<ExecutionPlan>>> GetBestExecution([FromBody] ExecutionRequest request)
    {
        try
        {
            _logger.LogInformation("Processing best execution request for {OrderType} {Amount} BTC",
                request.OrderType, request.Amount);

            // Manual validation with FluentValidation
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<ErrorResponse>
                {
                    Success = false,
                    Data = new ErrorResponse
                    {
                        Message = "Validation failed",
                        Errors = errors
                    },
                    Message = "Validation failed"
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
            return BadRequest(new ApiResponse<ErrorResponse>
            {
                Success = false,
                Data = new ErrorResponse
                {
                    Message = "Invalid request parameters",
                    Errors = new List<string> { ex.Message }
                },
                Message = "Invalid request parameters"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing best execution request");
            return StatusCode(500, new ApiResponse<ErrorResponse>
            {
                Success = false,
                Data = new ErrorResponse
                {
                    Message = "An internal error occurred while processing the request",
                    Errors = new List<string> { "Please try again later or contact support" }
                },
                Message = "An internal error occurred while processing the request"
            });
        }
    }
}
