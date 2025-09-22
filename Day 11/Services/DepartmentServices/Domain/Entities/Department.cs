public class Department
{
    public string Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;

    public void Initialize(string id, string name)
    {
        Id = string.IsNullOrWhiteSpace(id) ? throw new ArgumentException("Id is required") : id;
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Name = name.Trim();
    }
}
