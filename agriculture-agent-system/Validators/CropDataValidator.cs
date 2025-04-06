using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class CropDataValidator : AbstractValidator<CropData>
    {
        public CropDataValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Crop name is required")
                .Length(2, 50).WithMessage("Crop name must be between 2-50 characters");

            RuleFor(x => x.Season)
                .NotEmpty().WithMessage("Growing season is required")
                .Length(2, 20).WithMessage("Season must be between 2-20 characters");

            RuleFor(x => x.GrowthDays)
                .GreaterThan(0).WithMessage("Growth days must be positive")
                .LessThanOrEqualTo(365).WithMessage("Growth period cannot exceed 1 year");

            RuleFor(x => x.WaterRequirement)
                .GreaterThan(0).WithMessage("Water requirement must be positive");

            RuleFor(x => x.SoilType)
                .NotEmpty().WithMessage("Soil type is required")
                .Length(2, 30).WithMessage("Soil type must be between 2-30 characters");
        }
    }
}