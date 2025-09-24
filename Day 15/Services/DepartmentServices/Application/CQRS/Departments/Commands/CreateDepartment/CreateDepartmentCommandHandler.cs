using MediatR;
using MassTransit;
using SharedEvents.Events;

namespace DepartmentServices.Application.CQRS.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Department>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IPublishEndpoint publishEndpoint)
    {
        _departmentRepository = departmentRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Department> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department();
        department.Initialize(Guid.NewGuid().ToString(), request.Name);
        
        var createdDepartment = await _departmentRepository.AddAsync(department);

        // Publish event
        await _publishEndpoint.Publish(new DepartmentCreatedEvent
        {
            DepartmentId = createdDepartment.Id,
            Name = createdDepartment.Name,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return createdDepartment;
    }
}
