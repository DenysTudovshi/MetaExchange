using MetaExchange.Models;

namespace MetaExchange.Services;

public interface IBestExecutionService
{
    ExecutionPlan FindBestExecution(ExecutionRequest request);
}