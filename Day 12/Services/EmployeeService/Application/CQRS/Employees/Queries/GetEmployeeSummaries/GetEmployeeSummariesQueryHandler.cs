using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeSummaries;

public class GetEmployeeSummariesQueryHandler : IRequestHandler<GetEmployeeSummariesQuery, List<EmployeeSummary>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeSummariesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<EmployeeSummary>> Handle(GetEmployeeSummariesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(emp => new EmployeeSummary
        {
            Id = emp.Id,
            Name = emp.Name,
            Department = emp.DepartmentId ?? "No Department",
            Salary = emp.Salary.ToString()
        }).ToList();
    }
}
