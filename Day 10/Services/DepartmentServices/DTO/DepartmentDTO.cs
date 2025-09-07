public class DepartmentSummary
{
    public string Department { get; set; }
    public int EmployeeCount { get; set; }
    public double AverageAge { get; set; }
    public List<string> EmployeeNames { get; set; }
}

public class CreateDepartmentRequest
{
    public string Name { get; set; }
}