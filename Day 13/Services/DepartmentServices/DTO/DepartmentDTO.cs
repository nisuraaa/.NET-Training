public class DepartmentSummary
{
    public required string Department { get; set; }
    public required int EmployeeCount { get; set; }
    public required double AverageAge { get; set; }
    public required List<string> EmployeeNames { get; set; }
}

public class CreateDepartmentRequest
{
    public required string Name { get; set; }
}