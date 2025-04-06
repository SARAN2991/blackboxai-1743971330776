using AgricultureAgentSystem.Models;
using FluentValidation;

namespace AgricultureAgentSystem.Validators
{
    public class QueryLogValidator : AbstractValidator<QueryLog>
    {
        public QueryLogValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0");

            RuleFor(x => x.QueryType)
                .NotEmpty().WithMessage("Query type is required");

            RuleFor(x => x.Parameters)
                .NotEmpty().WithMessage("Parameters are required");

            RuleFor(x => x.Response)
                .NotEmpty().WithMessage("Response is required");
        }
    }
}