using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class QueryLogValidator : AbstractValidator<QueryLog>
    {
        public QueryLogValidator()
        {
            RuleFor(x => x.QueryType)
                .NotEmpty().WithMessage("Query type is required")
                .MaximumLength(100).WithMessage("Query type cannot exceed 100 characters");

            RuleFor(x => x.Parameters)
                .MaximumLength(500).WithMessage("Parameters cannot exceed 500 characters");

            RuleFor(x => x.Response)
                .MaximumLength(1000).WithMessage("Response cannot exceed 1000 characters");
        }
    }
}