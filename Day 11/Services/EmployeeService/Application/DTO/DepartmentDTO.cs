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

public class CreateDepartmentResponse
{
    public required string id {get; set;}
    public required string name {get; set;}

}

public class GetDepartmentResponse
{
    public required string id {get; set;}
    public required string name {get; set;}
}

