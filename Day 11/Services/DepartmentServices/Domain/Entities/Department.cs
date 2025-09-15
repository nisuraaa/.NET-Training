namespace DepartmentServices.Domain.Entities;

using DepartmentServices.Domain.ValueObjects;

public class Department
{
    public string Id { get; private set; }
    public DepartmentName Name { get; private set; }

    private Department() { /* EF Core */ }

    private Department(string id, DepartmentName name)
    {
        Id = id;
        Name = name;
    }

    public static Department Create(string name)
    {
        var id = Guid.NewGuid().ToString();
        return new Department(id, new DepartmentName(name));
    }

    public void Rename(string newName)
    {
        Name = new DepartmentName(newName);
    }
}
