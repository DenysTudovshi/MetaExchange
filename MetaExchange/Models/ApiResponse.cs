namespace MetaExchange.Models;

/// <summary>
/// Represents a standardized API response wrapper that provides consistent structure 
/// for all API endpoints, including success/failure status, data payload, and error handling.
/// </summary>
/// <typeparam name="T">The type of data being returned in the response payload</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the API operation completed successfully.
    /// True for successful operations (HTTP 200), false for errors (HTTP 400, 500, etc.)
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The actual response data payload. Contains the requested data when Success is true,
    /// or additional error information when Success is false. Will be null for failed operations
    /// that don't return specific error data.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// A human-readable message describing the result of the operation.
    /// Examples: "Best execution plan calculated successfully", "Validation failed", 
    /// "An internal error occurred while processing the request"
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// A collection of specific error messages or validation failures.
    /// Empty for successful operations. Contains detailed error descriptions
    /// such as validation errors, business rule violations, or system errors.
    /// </summary>
    public List<string> Errors { get; set; } = new();
}