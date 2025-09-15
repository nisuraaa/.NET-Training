namespace DepartmentServices.Domain.ValueObjects;

public readonly record struct DepartmentName
{
    public string Value { get; }

    public DepartmentName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Department name cannot be empty", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Department name cannot exceed 50 characters", nameof(value));
        Value = value.Trim();
    }

    public override string ToString() => Value;
}
