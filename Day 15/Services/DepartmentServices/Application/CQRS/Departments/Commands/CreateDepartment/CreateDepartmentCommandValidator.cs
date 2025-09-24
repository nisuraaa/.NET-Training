using FluentValidation;

namespace DepartmentServices.Application.CQRS.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");
    }
}
