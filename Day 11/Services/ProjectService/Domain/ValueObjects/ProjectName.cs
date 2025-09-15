namespace ProjectService.Domain.ValueObjects;

public readonly record struct ProjectName
{
    public string Value { get; }

    public ProjectName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Project name cannot be empty", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Project name cannot exceed 50 characters", nameof(value));
        Value = value.Trim();
    }

    public override string ToString() => Value;
}
