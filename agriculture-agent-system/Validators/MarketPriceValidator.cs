using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class MarketPriceValidator : AbstractValidator<MarketPrice>
    {
        public MarketPriceValidator()
        {
            RuleFor(x => x.Crop)
                .NotEmpty().WithMessage("Crop name is required")
                .Length(2, 50).WithMessage("Crop name must be between 2-50 characters");

            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2-100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be positive");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the future");
        }
    }
}