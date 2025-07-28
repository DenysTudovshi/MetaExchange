using FluentValidation;
using MetaExchange.Models;

namespace MetaExchange.Validators;
public class ExecutionRequestValidator : AbstractValidator<ExecutionRequest>
{
    public ExecutionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Exchanges)
            .NotEmpty()
            .WithMessage("At least one exchange must be provided");

        RuleForEach(x => x.Exchanges)
            .SetValidator(new ExchangeValidator());
    }
}
