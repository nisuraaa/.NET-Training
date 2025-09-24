using MassTransit;
using SharedEvents.Events;

namespace ProjectService.Application.EventHandlers;

public class DepartmentEventConsumer : IConsumer<DepartmentCreatedEvent>, IConsumer<DepartmentUpdatedEvent>, IConsumer<DepartmentDeletedEvent>
{
    private readonly ILogger<DepartmentEventConsumer> _logger;

    public DepartmentEventConsumer(ILogger<DepartmentEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DepartmentCreatedEvent> context)
    {
        var departmentEvent = context.Message;
        _logger.LogInformation("Department Created Event Received: {DepartmentId} - {Name}", 
            departmentEvent.DepartmentId, departmentEvent.Name);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<DepartmentUpdatedEvent> context)
    {
        var departmentEvent = context.Message;
        _logger.LogInformation("Department Updated Event Received: {DepartmentId} - {Name}", 
            departmentEvent.DepartmentId, departmentEvent.Name);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<DepartmentDeletedEvent> context)
    {
        var departmentEvent = context.Message;
        _logger.LogInformation("Department Deleted Event Received: {DepartmentId}", 
            departmentEvent.DepartmentId);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }
}
