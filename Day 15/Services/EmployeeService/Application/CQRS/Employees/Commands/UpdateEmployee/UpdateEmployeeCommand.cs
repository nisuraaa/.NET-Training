using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand : IRequest<Employee>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string? DepartmentName { get; set; }
}
