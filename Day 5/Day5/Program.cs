//Create a new C# Console Application.

using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

public class GenericRepository<T>
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> GetAll() => _items;
}

class Program
{
    static async Task Main(string[] args)
    {
        IEmployee employeeService = new EmployeeService();
        var employeeRepo = new GenericRepository<Employee>();
        var managerRepo = new GenericRepository<Manager>();

        while (true) //Use control flow statements (if, switch, for, foreach) to navigate a simple text-based menu.
        {
            Console.WriteLine("\n--- Employee Management System ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View All Employees");
            Console.WriteLine("3. Search Employee by ID");
            Console.WriteLine("4. Serialize Employees to JSON");
            Console.WriteLine("5. Get All Employees by Department");
            Console.WriteLine("6. Get All Employees by Age Range");
            Console.WriteLine("7. Exit");
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
                        await employeeService.AddEmployee(mgr);
                        managerRepo.Add(mgr);
                    }
                    else
                    {
                        var emp = new Employee { Id = id, Name = name, Age = age, Department = dept };
                        await employeeService.AddEmployee(emp);
                        employeeRepo.Add(emp);
                    }
                    break;
                case "2":
                    foreach (var emp in await employeeService.GetAllEmployees())
                        emp.DisplayDetails();
                    break;
                case "3":
                    Console.Write("Enter ID: ");
                    string searchId = Console.ReadLine();
                    var result = await employeeService.GetEmployeeByIdAsync(searchId);
                    result?.DisplayDetails();
                    break;
                case "4":
                    await SerializeEmployees(employeeService);
                    break;
                case "5":
                    Console.Write("Enter Department you want to search: ");
                    string department = Console.ReadLine();
                    if (string.IsNullOrEmpty(department))
                    {
                        Console.WriteLine("Department cannot be empty.");
                        continue;
                    }
                    var employeesByDept = await employeeService.GetAllEmployeesbyDepartment(department);
                    if (employeesByDept.Count == 0)
                    {
                        Console.WriteLine($"No employees found in department: {department}");
                    }
                    else
                    {
                        foreach (var emp in employeesByDept)
                            emp.DisplayDetails();
                    }
                    break;
                case "6":
                    Console.Write("Enter upper age limit: ");
                    int upperAgeLimit;
                    if (!int.TryParse(Console.ReadLine(), out upperAgeLimit) || upperAgeLimit < 0)
                    {
                        Console.WriteLine("Invalid age limit.");
                        continue;
                    }
                    Console.Write("Enter lower age limit: ");
                    int lowerAgeLimit;
                    if (!int.TryParse(Console.ReadLine(), out lowerAgeLimit) || lowerAgeLimit < 0)
                    {
                        Console.WriteLine("Invalid age limit.");
                        continue;
                    }
                    if (lowerAgeLimit > upperAgeLimit)
                    {
                        Console.WriteLine("Lower age limit cannot be greater than upper age limit.");
                        continue;
                    }
                    var employeesByAge = await employeeService.GetAllEmployeesbyAgeRange(lowerAgeLimit, upperAgeLimit);
                    if (employeesByAge.Count == 0)
                    {
                        Console.WriteLine($"No employees found in age range: {lowerAgeLimit} - {upperAgeLimit}");
                    }
                    else
                    {
                        foreach (var emp in employeesByAge)
                            emp.DisplayDetails();
                    }
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

    }

    static async Task SerializeEmployees(IEmployee employeeService)
    {
        await Task.Delay(3000); // Simulate async operation
        var employees = await employeeService.GetAllEmployees();
        string json = JsonConvert.SerializeObject(employees, Newtonsoft.Json.Formatting.Indented);
        Console.WriteLine(json);
    }
}