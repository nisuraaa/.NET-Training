public class Project
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
