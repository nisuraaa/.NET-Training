using MediatR;
using MassTransit;
using SharedEvents.Events;

namespace DepartmentServices.Application.CQRS.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, bool>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository, IPublishEndpoint publishEndpoint)
    {
        _departmentRepository = departmentRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetByIdAsync(request.Id);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with ID {request.Id} not found");
        }

        var result = await _departmentRepository.DeleteAsync(request.Id);

        if (result)
        {
            // Publish event
            await _publishEndpoint.Publish(new DepartmentDeletedEvent
            {
                DepartmentId = request.Id,
                DeletedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        return result;
    }
}
