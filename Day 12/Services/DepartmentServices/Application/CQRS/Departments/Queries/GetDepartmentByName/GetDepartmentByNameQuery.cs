using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Queries.GetDepartmentByName;

public class GetDepartmentByNameQuery : IRequest<Department?>
{
    public string Name { get; set; } = string.Empty;
}
