using FluentValidation;

namespace EmployeeServices.Application.CQRS.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Age must be 18 or older")
            .LessThanOrEqualTo(100).WithMessage("Age cannot exceed 100");

        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0).WithMessage("Salary cannot be negative");

        RuleFor(x => x.DepartmentName)
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.DepartmentName));
    }
}
