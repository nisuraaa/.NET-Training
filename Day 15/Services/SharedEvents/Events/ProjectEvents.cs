namespace SharedEvents.Events;

// Project Created Event
public record ProjectCreatedEvent
{
    public string ProjectId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

// Project Updated Event
public record ProjectUpdatedEvent
{
    public string ProjectId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public DateTime UpdatedAt { get; init; }
}

// Project Deleted Event
public record ProjectDeletedEvent
{
    public string ProjectId { get; init; } = string.Empty;
    public DateTime DeletedAt { get; init; }
}
