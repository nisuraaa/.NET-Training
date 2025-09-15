namespace EmployeeServices.Domain.ValueObjects;

public readonly record struct EmployeeName
{
    public string Value { get; }

    public EmployeeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Employee name cannot be empty", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Employee name cannot exceed 100 characters", nameof(value));
        Value = value.Trim();
    }

    public override string ToString() => Value;
}
