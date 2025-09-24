using MediatR;
using MassTransit;
using SharedEvents.Events;

namespace DepartmentServices.Application.CQRS.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Department>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IPublishEndpoint publishEndpoint)
    {
        _departmentRepository = departmentRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Department> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var existingDepartment = await _departmentRepository.GetByIdAsync(request.Id);
        if (existingDepartment == null)
        {
            throw new KeyNotFoundException($"Department with ID {request.Id} not found");
        }

        existingDepartment.SetName(request.Name);
        var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment);

        // Publish event
        await _publishEndpoint.Publish(new DepartmentUpdatedEvent
        {
            DepartmentId = updatedDepartment.Id,
            Name = updatedDepartment.Name,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);

        return updatedDepartment;
    }
}
