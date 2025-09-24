using MediatR;
using MassTransit;
using SharedEvents.Events;

namespace EmployeeServices.Application.CQRS.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Employee>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentHttpClient _departmentHttpClient;
    private readonly IProjectHttpClient _projectHttpClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentHttpClient departmentHttpClient,
        IProjectHttpClient projectHttpClient,
        IPublishEndpoint publishEndpoint)
    {
        _employeeRepository = employeeRepository;
        _departmentHttpClient = departmentHttpClient;
        _projectHttpClient = projectHttpClient;
        _publishEndpoint = publishEndpoint;
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

        var createdEmployee = await _employeeRepository.AddAsync(employee);

        // Publish event
        await _publishEndpoint.Publish(new EmployeeCreatedEvent
        {
            EmployeeId = createdEmployee.Id,
            Name = createdEmployee.Name,
            Age = createdEmployee.Age,
            DepartmentId = createdEmployee.DepartmentId ?? string.Empty,
            IsManager = request.IsManager,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return createdEmployee;
    }
}
