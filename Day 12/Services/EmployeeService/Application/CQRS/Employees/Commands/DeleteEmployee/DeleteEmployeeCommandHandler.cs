using MediatR;

namespace EmployeeServices.Application.CQRS.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");
        }

        await _employeeRepository.DeleteAsync(employee.Id);
        return Unit.Value;
    }
}
