using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class MarketPriceQueryValidator : AbstractValidator<MarketPriceQuery>
    {
        public MarketPriceQueryValidator()
        {
            RuleFor(x => x.Crop)
                .NotEmpty().WithMessage("Crop name is required")
                .Length(2, 50).WithMessage("Crop name must be between 2 and 50 characters");

            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2 and 100 characters");

            RuleFor(x => x.Days)
                .GreaterThan(0).WithMessage("Days must be greater than 0")
                .LessThanOrEqualTo(365).WithMessage("Cannot query more than 1 year of data");
        }
    }
}