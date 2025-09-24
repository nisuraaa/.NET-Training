public class Employee
{
    public string Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public int Age { get; private set; }
    public decimal Salary { get; private set; }
    public string? DepartmentId { get; private set; }
    public List<string> ProjectIds { get; private set; } = new();

    // Factory/helper for creation if needed
    public void Initialize(string id, string name, int age, decimal salary)
    {
        Id = string.IsNullOrWhiteSpace(id) ? throw new ArgumentException("Id is required") : id;
        SetName(name);
        SetAge(age);
        SetSalary(salary);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Name = name.Trim();
    }

    public void SetAge(int age)
    {
        if (age < 18) throw new ArgumentException("Age must be 18 or older");
        Age = age;
    }

    public void SetSalary(decimal salary)
    {
        if (salary < 0) throw new ArgumentException("Salary cannot be negative");
        Salary = salary;
    }

    public void AssignDepartment(string departmentId)
    {
        if (string.IsNullOrWhiteSpace(departmentId)) throw new ArgumentException("DepartmentId is required");
        DepartmentId = departmentId;
    }

    public void AddProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("ProjectId is required");
        if (!ProjectIds.Contains(projectId))
        {
            ProjectIds.Add(projectId);
        }
    }

    public void RemoveProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId)) return;
        ProjectIds.Remove(projectId);
    }
}