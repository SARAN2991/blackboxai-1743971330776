using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class WeatherDataValidator : AbstractValidator<WeatherData>
    {
        public WeatherDataValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2 and 100 characters");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the future");

            RuleFor(x => x.Temperature)
                .NotNull().WithMessage("Temperature is required");

            RuleFor(x => x.Humidity)
                .NotNull().WithMessage("Humidity is required");

            RuleFor(x => x.Rainfall)
                .NotNull().WithMessage("Rainfall is required");

            RuleFor(x => x.WindSpeed)
                .NotNull().WithMessage("Wind speed is required");
        }
    }
}