using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<Employee>
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string IsManager { get; set; } = "false";
    public int? TeamCount { get; set; } = 0;
    public string? DepartmentName { get; set; }
    public string? ProjectName { get; set; }
}
