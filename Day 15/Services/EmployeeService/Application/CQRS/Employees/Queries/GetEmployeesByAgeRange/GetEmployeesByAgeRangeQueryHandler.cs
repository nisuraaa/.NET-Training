using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeesByAgeRange;

public class GetEmployeesByAgeRangeQueryHandler : IRequestHandler<GetEmployeesByAgeRangeQuery, List<Employee>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeesByAgeRangeQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<List<Employee>> Handle(GetEmployeesByAgeRangeQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Where(e => e.Age >= request.LowerAgeLimit && e.Age <= request.UpperAgeLimit).ToList();
    }
}
