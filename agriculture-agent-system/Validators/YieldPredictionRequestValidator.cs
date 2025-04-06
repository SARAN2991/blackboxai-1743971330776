using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class YieldPredictionRequestValidator : AbstractValidator<YieldPredictionRequest>
    {
        public YieldPredictionRequestValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2 and 100 characters");

            RuleFor(x => x.Crop)
                .NotEmpty().WithMessage("Crop type is required")
                .Length(2, 50).WithMessage("Crop type must be between 2 and 50 characters");

            RuleFor(x => x.SoilData)
                .NotEmpty().WithMessage("Soil data is required")
                .Must(BeValidJson).WithMessage("Soil data must be valid JSON");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Area cannot exceed 10,000 hectares");
        }

        private bool BeValidJson(string json)
        {
            try 
            {
                System.Text.Json.JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}