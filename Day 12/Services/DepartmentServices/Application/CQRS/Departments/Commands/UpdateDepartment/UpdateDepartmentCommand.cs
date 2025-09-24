using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommand : IRequest<Department>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
