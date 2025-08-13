public class Employee
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public string? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

}