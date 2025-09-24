using MassTransit;
using SharedEvents.Events;

namespace EmployeeServices.Application.EventHandlers;

public class ProjectEventConsumer : IConsumer<ProjectCreatedEvent>, IConsumer<ProjectUpdatedEvent>, IConsumer<ProjectDeletedEvent>
{
    private readonly ILogger<ProjectEventConsumer> _logger;

    public ProjectEventConsumer(ILogger<ProjectEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
    {
        var projectEvent = context.Message;
        _logger.LogInformation("Project Created Event Received: {ProjectId} - {Name}", 
            projectEvent.ProjectId, projectEvent.Name);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<ProjectUpdatedEvent> context)
    {
        var projectEvent = context.Message;
        _logger.LogInformation("Project Updated Event Received: {ProjectId} - {Name}", 
            projectEvent.ProjectId, projectEvent.Name);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
    {
        var projectEvent = context.Message;
        _logger.LogInformation("Project Deleted Event Received: {ProjectId}", 
            projectEvent.ProjectId);
        
        // Here you could update local cache, send notifications, etc.
        // You might want to remove this project from employees' project lists
        await Task.CompletedTask;
    }
}
