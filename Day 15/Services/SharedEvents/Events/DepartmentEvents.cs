namespace SharedEvents.Events;

// Department Created Event
public record DepartmentCreatedEvent
{
    public string DepartmentId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

// Department Updated Event
public record DepartmentUpdatedEvent
{
    public string DepartmentId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime UpdatedAt { get; init; }
}

// Department Deleted Event
public record DepartmentDeletedEvent
{
    public string DepartmentId { get; init; } = string.Empty;
    public DateTime DeletedAt { get; init; }
}
