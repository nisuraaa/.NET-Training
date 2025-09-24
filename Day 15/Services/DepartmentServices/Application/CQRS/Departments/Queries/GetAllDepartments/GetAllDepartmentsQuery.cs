using MediatR;

namespace DepartmentServices.Application.CQRS.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQuery : IRequest<IEnumerable<Department>>
{
}
