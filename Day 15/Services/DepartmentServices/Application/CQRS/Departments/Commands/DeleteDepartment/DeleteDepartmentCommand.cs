using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommand : IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
}
