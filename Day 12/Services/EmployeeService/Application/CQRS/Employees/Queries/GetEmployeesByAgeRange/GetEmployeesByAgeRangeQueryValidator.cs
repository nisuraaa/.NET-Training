using FluentValidation;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeesByAgeRange;

public class GetEmployeesByAgeRangeQueryValidator : AbstractValidator<GetEmployeesByAgeRangeQuery>
{
    public GetEmployeesByAgeRangeQueryValidator()
    {
        RuleFor(x => x.LowerAgeLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Lower age limit cannot be negative");

        RuleFor(x => x.UpperAgeLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Upper age limit cannot be negative");

        RuleFor(x => x.UpperAgeLimit)
            .GreaterThanOrEqualTo(x => x.LowerAgeLimit)
            .WithMessage("Upper age limit must be greater than or equal to lower age limit");
    }
}
