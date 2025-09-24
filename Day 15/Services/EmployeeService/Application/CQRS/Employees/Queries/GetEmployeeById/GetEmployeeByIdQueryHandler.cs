using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee?>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _employeeRepository.GetByIdAsync(request.Id);
    }
}
