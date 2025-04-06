using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class DiseaseDetectionRequestValidator : AbstractValidator<DiseaseDetectionRequest>
    {
        public DiseaseDetectionRequestValidator()
        {
            RuleFor(x => x.PlantType)
                .NotEmpty().WithMessage("Plant type is required")
                .Length(2, 50).WithMessage("Plant type must be between 2 and 50 characters");

            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Image file is required");
        }
    }
}