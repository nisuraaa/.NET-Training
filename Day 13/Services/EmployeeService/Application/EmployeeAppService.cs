using MediatR;
using EmployeeServices.Application.CQRS.Employees.Commands.CreateEmployee;
using EmployeeServices.Application.CQRS.Employees.Commands.UpdateEmployee;
using EmployeeServices.Application.CQRS.Employees.Commands.DeleteEmployee;
using EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeById;
using EmployeeServices.Application.CQRS.Employees.Queries.GetAllEmployees;
using EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeesByAgeRange;
using EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeSummaries;

namespace EmployeeServices.Application;

public class EmployeeAppService : IEmployeeAppService
{
    private readonly IMediator _mediator;

    public EmployeeAppService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Employee> AddEmployee(CreateEmployeeRequest employeeRequest)
    {
        var command = new CreateEmployeeCommand
        {
            Name = employeeRequest.Name,
            Age = employeeRequest.Age,
            Salary = employeeRequest.Salary,
            IsManager = employeeRequest.isManager,
            TeamCount = employeeRequest.teamCount,
            DepartmentName = employeeRequest.DepartmentName,
            ProjectName = employeeRequest.ProjectName
        };

        return await _mediator.Send(command);
    }

    public async Task<Employee> UpdateEmployee(UpdateEmployeeRequest request)
    {
        var command = new UpdateEmployeeCommand
        {
            Id = request.Id,
            Name = request.Name,
            Age = request.Age,
            Salary = request.Salary,
            DepartmentName = request.DepartmentName
        };

        return await _mediator.Send(command);
    }

    public async Task DeleteEmployee(string id)
    {
        var command = new DeleteEmployeeCommand { Id = id };
        await _mediator.Send(command);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(string id)
    {
        var query = new GetEmployeeByIdQuery { Id = id };
        return await _mediator.Send(query);
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var query = new GetAllEmployeesQuery();
        return await _mediator.Send(query);
    }

    public async Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit)
    {
        var query = new GetEmployeesByAgeRangeQuery
        {
            LowerAgeLimit = lowerAgeLimit,
            UpperAgeLimit = upperAgeLimit
        };
        return await _mediator.Send(query);
    }

    public async Task<List<EmployeeSummary>> GetEmployeeSummaries()
    {
        var query = new GetEmployeeSummariesQuery();
        return await _mediator.Send(query);
    }

    public async Task<List<EmployeeBasicInfo>> GetEmployeeBasicInfo()
    {
        var employees = await GetAllEmployees();
        return employees.Select(emp => new EmployeeBasicInfo
        {
            Name = emp.Name,
            Age = emp.Age,
        }).ToList();
    }

    public async Task<List<EmployeeContactInfo>> GetEmployeeContactInfo()
    {
        var employees = await GetAllEmployees();
        return employees.Select(emp => new EmployeeContactInfo
        {
            FullName = emp.Name,
            DepartmentInfo = emp.DepartmentId != null ? $"{emp.DepartmentId} (ID: {emp.DepartmentId})" : "No Department"
        }).OrderBy(info => info.FullName).ToList();
    }

    public async Task<List<string>> GetEmployeeNames()
    {
        var employees = await GetAllEmployees();
        return employees.Select(e => e.Name).ToList();
    }

    public async Task<decimal> GetSummaryofSalaries()
    {
        var employees = await GetAllEmployees();
        return employees.Sum(e => e.Salary);
    }

    public async Task<double> GetAverageAgeofEmployees()
    {
        var employees = await GetAllEmployees();
        return employees.Average(e => (double)e.Age);
    }
}
