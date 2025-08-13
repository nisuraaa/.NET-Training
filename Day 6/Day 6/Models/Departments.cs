public class Department
{
    public string Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

