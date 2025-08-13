public class Employee
{
    //Define an Employee class with properties (Id, Name, Age, Department).

    private string _id;
    private string _name;
    private int _age;
    private string _department;

    public string Id { get => _id; set => _id = value ?? "Unknown"; }
    public string Name { get => _name; set => _name = value ?? "Unknown"; }
    public int Age { get => _age; set => _age = value >= 0 ? value : throw new ArgumentException("Invalid Age"); }
    public string Department { get => _department; set => _department = value ?? "General"; }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"[Employee] ID={Id}, Name={Name}, Age={Age}, Dept={Department}");
    }
}