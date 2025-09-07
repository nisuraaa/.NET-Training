public class Employee
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string? DepartmentId { get; set; }
    public List<string> ProjectIds { get; set; } = new();

}