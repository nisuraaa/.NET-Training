namespace EmployeeServices.Domain.Entities;

using EmployeeServices.Domain.ValueObjects;

public class Employee
{
    public string Id { get; private set; }
    public EmployeeName Name { get; private set; }
    public int Age { get; private set; }
    public decimal Salary { get; private set; }
    public string? DepartmentId { get; private set; }
    public List<string> ProjectIds { get; private set; } = new();

    private Employee() { /* EF Core */ }

    private Employee(string id, EmployeeName name, int age, decimal salary)
    {
        if (age < 0) throw new ArgumentOutOfRangeException(nameof(age));
        if (salary < 0) throw new ArgumentOutOfRangeException(nameof(salary));

        Id = id;
        Name = name;
        Age = age;
        Salary = salary;
    }

    public static Employee Hire(string name, int age, decimal salary)
    {
        return new Employee(Guid.NewGuid().ToString(), new EmployeeName(name), age, salary);
    }

    public void UpdateProfile(string name, int age)
    {
        if (age < 0) throw new ArgumentOutOfRangeException(nameof(age));
        Name = new EmployeeName(name);
        Age = age;
    }

    public void UpdateCompensation(decimal salary)
    {
        if (salary < 0) throw new ArgumentOutOfRangeException(nameof(salary));
        Salary = salary;
    }

    public void AssignDepartment(string departmentId)
    {
        if (string.IsNullOrWhiteSpace(departmentId))
            throw new ArgumentException("DepartmentId cannot be empty", nameof(departmentId));
        DepartmentId = departmentId;
    }

    public void AssignToProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
            throw new ArgumentException("ProjectId cannot be empty", nameof(projectId));
        if (!ProjectIds.Contains(projectId))
        {
            ProjectIds.Add(projectId);
        }
    }
}
