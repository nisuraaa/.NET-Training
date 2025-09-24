using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentHttpClient _departmentHttpClient;
    private readonly IProjectHttpClient _projectHttpClient;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentHttpClient departmentHttpClient,
        IProjectHttpClient projectHttpClient)
    {
        _employeeRepository = employeeRepository;
        _departmentHttpClient = departmentHttpClient;
        _projectHttpClient = projectHttpClient;
    }

    public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee employee;

        if (string.Equals(request.IsManager, "true", StringComparison.OrdinalIgnoreCase))
        {
            var mgr = new Manager
            {
                TeamSize = request.TeamCount ?? 0
            };
            mgr.Initialize(Guid.NewGuid().ToString(), request.Name, request.Age, request.Salary);
            employee = mgr;
        }
        else
        {
            employee = new Employee();
            employee.Initialize(Guid.NewGuid().ToString(), request.Name, request.Age, request.Salary);
        }

        if (request.DepartmentName != null)
        {
            var existingDept = await _departmentHttpClient.GetDepartmentByNameAsync(request.DepartmentName);
            if (existingDept == null)
            {
                var status = await _departmentHttpClient.CreateDepartmentAsync(request.DepartmentName);
                if (status != null)
                {
                    employee.AssignDepartment(status.id);
                }
            }
            else
            {
                employee.AssignDepartment(existingDept.id);
            }
        }

        if (request.ProjectName != null)
        {
            var existingProject = await _projectHttpClient.GetProjectByNameAsync(request.ProjectName);
            if (existingProject == null)
            {
                var projectResponse = await _projectHttpClient.CreateProjectAsync(request.ProjectName);
                if (projectResponse != null)
                {
                    employee.AddProject(projectResponse.id);
                }
            }
            else
            {
                employee.AddProject(existingProject.id);
            }
        }

        return await _employeeRepository.AddAsync(employee);
    }
}
