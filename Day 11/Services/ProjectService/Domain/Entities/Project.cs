namespace ProjectService.Domain.Entities;

using ProjectService.Domain.ValueObjects;

public class Project
{
    public string Id { get; private set; }
    public ProjectName Name { get; private set; }

    private Project() { /* EF Core */ }

    private Project(string id, ProjectName name)
    {
        Id = id;
        Name = name;
    }

    public static Project Create(string name)
    {
        var id = Guid.NewGuid().ToString();
        return new Project(id, new ProjectName(name));
    }

    public void Rename(string newName)
    {
        Name = new ProjectName(newName);
    }
}
