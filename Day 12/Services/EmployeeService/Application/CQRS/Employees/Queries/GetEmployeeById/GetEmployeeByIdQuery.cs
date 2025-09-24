using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQuery : IRequest<Employee?>
{
    public string Id { get; set; } = string.Empty;
}
