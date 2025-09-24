using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentHttpClient _departmentHttpClient;

    public UpdateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentHttpClient departmentHttpClient)
    {
        _employeeRepository = employeeRepository;
        _departmentHttpClient = departmentHttpClient;
    }

    public async Task<Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(request.Id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");
        }

        existingEmployee.SetName(request.Name);
        existingEmployee.SetAge(request.Age);
        existingEmployee.SetSalary(request.Salary);

        if (!string.IsNullOrEmpty(request.DepartmentName))
        {
            var department = await _departmentHttpClient.GetDepartmentByNameAsync(request.DepartmentName);
            if (department != null)
            {
                existingEmployee.AssignDepartment(department.id);
            }
            else
            {
                var created = await _departmentHttpClient.CreateDepartmentAsync(request.DepartmentName);
                if (created != null)
                {
                    existingEmployee.AssignDepartment(created.id);
                }
            }
        }

        return await _employeeRepository.UpdateAsync(existingEmployee);
    }
}
