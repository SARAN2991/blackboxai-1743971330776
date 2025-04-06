using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class CropDataValidator : AbstractValidator<CropData>
    {
        public CropDataValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(2, 100).WithMessage("Region must be between 2 and 100 characters");

            RuleFor(x => x.CropName)
                .NotEmpty().WithMessage("Crop name is required")
                .Length(2, 50).WithMessage("Crop name must be between 2 and 50 characters");

            RuleFor(x => x.SoilType)
                .NotEmpty().WithMessage("Soil type is required");

            RuleFor(x => x.OptimalSeason)
                .NotEmpty().WithMessage("Optimal season is required");

            RuleFor(x => x.WaterRequirement)
                .GreaterThan(0).WithMessage("Water requirement must be greater than 0");

            RuleFor(x => x.YieldPotential)
                .GreaterThan(0).WithMessage("Yield potential must be greater than 0");
        }
    }
}