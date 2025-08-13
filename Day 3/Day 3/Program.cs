using System.Xml;
using Newtonsoft.Json;


public interface IEmployee //Create an IEmployeeService interface defining methods like AddEmployee, GetEmployeeById, and GetAllEmployees.
{
    void AddEmployee(Employee employee);
    Employee GetEmployeeById(string id);
    List<Employee> GetAllEmployees();
}

public class EmployeeService : IEmployee
{
    private readonly Dictionary<string, Employee> _employees = new();

    public void AddEmployee(Employee emp)
    {
        if (!_employees.ContainsKey(emp.Id))
            _employees[emp.Id] = emp;
    }

    public Employee GetEmployeeById(string id) =>
        _employees.TryGetValue(id, out var emp) ? emp : null;

    public List<Employee> GetAllEmployees() => new List<Employee>(_employees.Values);
}

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
        Console.WriteLine($"[Employee] ID={Id}, Name={  Name}, Age={Age}, Dept={Department}");
    }
}

public class Manager : Employee
{
    public int TeamSize { get; set; }

    public override void DisplayDetails()
    {
        Console.WriteLine($"[Manager] ID={Id}, Name={Name}, Age={Age}, Dept={Department}, TeamSize={TeamSize}");
    }
}

public class GenericRepository<T>
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> GetAll() => _items;
}

public class Program
{
    static void Main(string[] args)
    {
        IEmployee employeeService = new EmployeeService();
        var employeeRepo = new GenericRepository<Employee>();
        var managerRepo = new GenericRepository<Manager>();

        while (true)
        {
            Console.WriteLine("\n--- Employee Management System ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View All Employees");
            Console.WriteLine("3. Search Employee by ID");
            Console.WriteLine("4. Serialize Employees to JSON");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Is this a Manager? (y/n): ");
                    bool isManager = Console.ReadLine()?.ToLower() == "y";

                    Console.Write("Enter ID: ");
                    string id = Console.ReadLine();

                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter Age: ");
                    int age = int.Parse(Console.ReadLine());

                    Console.Write("Enter Department: ");
                    string dept = Console.ReadLine();

                    if (isManager)
                    {
                        Console.Write("Enter Team Size: ");
                        int teamSize = int.Parse(Console.ReadLine());

                        var mgr = new Manager { Id = id, Name = name, Age = age, Department = dept, TeamSize = teamSize };
                        employeeService.AddEmployee(mgr);
                        managerRepo.Add(mgr);
                    }
                    else
                    {
                        var emp = new Employee { Id = id, Name = name, Age = age, Department = dept };
                        employeeService.AddEmployee(emp);
                        employeeRepo.Add(emp);
                    }
                    break;
                case "2":
                    foreach (var emp in employeeService.GetAllEmployees())
                        emp.DisplayDetails();
                    break;
                case "3":
                    Console.Write("Enter ID: ");
                    string searchId = Console.ReadLine();
                    var result = employeeService.GetEmployeeById(searchId);
                    result?.DisplayDetails();
                    break;
                case "4":
                    SerializeEmployees(employeeService);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

    }

    static void SerializeEmployees(IEmployee employeeService)
    {
        var employees = employeeService.GetAllEmployees();
        string json = JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented);
        Console.WriteLine(json);
    }
}