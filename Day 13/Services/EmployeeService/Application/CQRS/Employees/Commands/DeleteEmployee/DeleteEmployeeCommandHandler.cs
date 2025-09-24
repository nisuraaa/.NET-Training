using MediatR;
using MassTransit;
using SharedEvents.Events;

namespace EmployeeServices.Application.CQRS.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository, IPublishEndpoint publishEndpoint)
    {
        _employeeRepository = employeeRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");
        }

        await _employeeRepository.DeleteAsync(employee.Id);

        // Publish event
        await _publishEndpoint.Publish(new EmployeeDeletedEvent
        {
            EmployeeId = request.Id,
            DeletedAt = DateTime.UtcNow
        }, cancellationToken);

        return Unit.Value;
    }
}
