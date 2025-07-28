namespace MetaExchange.Models;

/// <summary>
/// Represents detailed error information for failed API operations.
/// Used as the data payload in ApiResponse&lt;ErrorResponse&gt; when operations fail,
/// providing structured error details including a summary message and specific error descriptions.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// A high-level summary message describing the nature of the error or failure.
    /// Examples: "Validation failed", "Invalid request parameters", 
    /// "An internal error occurred while processing the request"
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// A detailed collection of specific error messages, validation failures, or issue descriptions.
    /// Each item represents a distinct problem that occurred during request processing.
    /// Examples: "Amount must be greater than zero", "ExchangeId is required", 
    /// "EUR balance cannot be negative"
    /// </summary>
    public List<string> Errors { get; set; } = new();
}