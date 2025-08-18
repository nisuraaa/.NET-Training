using FluentValidation;

namespace EmployeeManagement.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required")
            .Length(2, 100).WithMessage("Employee name must be between 2 and 100 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Employee name can only contain letters and spaces");

        RuleFor(x => x.Age)
            .InclusiveBetween(18, 100).WithMessage("Employee age must be between 18 and 100");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("Salary cannot exceed 1,000,000");

        RuleFor(x => x.DepartmentName)
            .MaximumLength(50).WithMessage("Department name cannot exceed 50 characters");
    }
}
