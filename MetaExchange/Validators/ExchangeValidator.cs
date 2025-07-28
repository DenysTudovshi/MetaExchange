using FluentValidation;
using MetaExchange.Models;

namespace MetaExchange.Validators;
public class ExchangeValidator : AbstractValidator<Exchange>
{
    public ExchangeValidator()
    {
        RuleFor(x => x.ExchangeId)
            .NotEmpty()
            .WithMessage("ExchangeId is required");

        RuleFor(x => x.Balance.EUR)
            .GreaterThanOrEqualTo(0)
            .WithMessage("EUR balance cannot be negative");

        RuleFor(x => x.Balance.BTC)
            .GreaterThanOrEqualTo(0)
            .WithMessage("BTC balance cannot be negative");

        RuleFor(x => x.OrderBook)
            .Must(HaveOrders)
            .WithMessage("Must have at least one bid or ask order");
    }

    private bool HaveOrders(OrderBook orderBook)
    {
        return (orderBook.Bids != null && orderBook.Bids.Any()) ||
               (orderBook.Asks != null && orderBook.Asks.Any());
    }
}
