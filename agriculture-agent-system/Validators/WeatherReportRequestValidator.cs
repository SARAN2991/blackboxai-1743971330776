using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class WeatherReportRequestValidator : AbstractValidator<WeatherReportRequest>
    {
        public WeatherReportRequestValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2 and 100 characters");

            RuleFor(x => x.Days)
                .GreaterThan(0).WithMessage("Days must be greater than 0")
                .LessThanOrEqualTo(14).WithMessage("Cannot forecast more than 14 days");
        }
    }
}