namespace SharedEvents.Events;

// Employee Created Event
public record EmployeeCreatedEvent
{
    public string EmployeeId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string DepartmentId { get; init; } = string.Empty;
    public string IsManager { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

// Employee Updated Event
public record EmployeeUpdatedEvent
{
    public string EmployeeId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string DepartmentId { get; init; } = string.Empty;
    public string IsManager { get; init; } = string.Empty;
    public DateTime UpdatedAt { get; init; }
}

// Employee Deleted Event
public record EmployeeDeletedEvent
{
    public string EmployeeId { get; init; } = string.Empty;
    public DateTime DeletedAt { get; init; }
}
