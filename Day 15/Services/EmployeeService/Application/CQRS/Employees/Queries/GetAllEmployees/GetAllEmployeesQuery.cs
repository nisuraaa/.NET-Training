using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetAllEmployees;

public class GetAllEmployeesQuery : IRequest<List<Employee>>
{
}
