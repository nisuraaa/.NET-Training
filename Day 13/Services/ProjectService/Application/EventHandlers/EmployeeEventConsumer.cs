using MassTransit;
using SharedEvents.Events;

namespace ProjectService.Application.EventHandlers;

public class EmployeeEventConsumer : IConsumer<EmployeeCreatedEvent>, IConsumer<EmployeeUpdatedEvent>, IConsumer<EmployeeDeletedEvent>
{
    private readonly ILogger<EmployeeEventConsumer> _logger;

    public EmployeeEventConsumer(ILogger<EmployeeEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EmployeeCreatedEvent> context)
    {
        var employeeEvent = context.Message;
        _logger.LogInformation("Employee Created Event Received: {EmployeeId} - {Name} - Department: {DepartmentId}", 
            employeeEvent.EmployeeId, employeeEvent.Name, employeeEvent.DepartmentId);
        
        // Here you could update local cache, send notifications, etc.
        // You might want to track which employees are assigned to which projects
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<EmployeeUpdatedEvent> context)
    {
        var employeeEvent = context.Message;
        _logger.LogInformation("Employee Updated Event Received: {EmployeeId} - {Name} - Department: {DepartmentId}", 
            employeeEvent.EmployeeId, employeeEvent.Name, employeeEvent.DepartmentId);
        
        // Here you could update local cache, send notifications, etc.
        await Task.CompletedTask;
    }

    public async Task Consume(ConsumeContext<EmployeeDeletedEvent> context)
    {
        var employeeEvent = context.Message;
        _logger.LogInformation("Employee Deleted Event Received: {EmployeeId}", 
            employeeEvent.EmployeeId);
        
        // Here you could update local cache, send notifications, etc.
        // You might want to remove this employee from project assignments
        await Task.CompletedTask;
    }
}
