using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommand : IRequest<Department>
{
    public string Name { get; set; } = string.Empty;
}
