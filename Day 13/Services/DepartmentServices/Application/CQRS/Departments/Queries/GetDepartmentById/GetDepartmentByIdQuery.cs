using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQuery : IRequest<Department?>
{
    public string Id { get; set; } = string.Empty;
}
