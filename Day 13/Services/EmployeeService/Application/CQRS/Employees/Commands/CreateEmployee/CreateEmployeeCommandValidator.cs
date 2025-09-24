using FluentValidation;

namespace EmployeeServices.Application.CQRS.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Age must be 18 or older")
            .LessThanOrEqualTo(100).WithMessage("Age cannot exceed 100");

        RuleFor(x => x.Salary)
            .GreaterThanOrEqualTo(0).WithMessage("Salary cannot be negative");

        RuleFor(x => x.IsManager)
            .Must(x => x == "true" || x == "false")
            .WithMessage("IsManager must be 'true' or 'false'");

        RuleFor(x => x.TeamCount)
            .GreaterThanOrEqualTo(0).WithMessage("Team count cannot be negative")
            .When(x => x.IsManager == "true");

        RuleFor(x => x.DepartmentName)
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.DepartmentName));

        RuleFor(x => x.ProjectName)
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ProjectName));
    }
}
