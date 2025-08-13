using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

public class GenericRepository<T>
{
    private readonly List<T> _items = new();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> GetAll() => _items;
}

public static class EmployeeExtensions
{
    // Formatting employee names
    public static string FormatName(this Employee employee)
    {
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(employee.Name.ToLower());
    }
    // Filtering employees by department
    public static IEnumerable<Employee> FilterByDepartment(this IEnumerable<Employee> employees, string department)
    {
        return employees.Where(emp => emp.Department != null && emp.Department.Name.Equals(department, StringComparison.OrdinalIgnoreCase));
    }

    // Calculating average age of employees

    public static double CalculateAverageAge(this IEnumerable<Employee> employees)
    {
        return employees.Any() ? employees.Average(emp => emp.Age) : 0;
    }

    public static void DisplayDetails(this Employee employee)
    {
        Console.WriteLine($"ID: {employee.Id}");
        Console.WriteLine($"Name: {employee.Name}");
        Console.WriteLine($"Age: {employee.Age}");
        Console.WriteLine($"Salary: {employee.Salary:C}");
        Console.WriteLine($"Department: {employee.Department?.Name ?? "No Department"}");
    }
}


class Program
{
    static async Task Main(string[] args)
    {

        // Registering IEmployee and GenericRepository services using Dependency Injection
        var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services.AddSingleton<IEmployee, EmployeeService>();
            services.AddSingleton<IProject, ProjectService>();
            services.AddSingleton<GenericRepository<Employee>>();
            services.AddSingleton<GenericRepository<Manager>>();
        })
        .Build();

        // Create database and tables if they don't exist
        using (var context = new dbContext())
        {
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Database initialized successfully!");
        }
        var employeeService = host.Services.GetRequiredService<IEmployee>();
        var projectService = host.Services.GetRequiredService<IProject>();
        var employeeRepo = host.Services.GetRequiredService<GenericRepository<Employee>>();
        var managerRepo = host.Services.GetRequiredService<GenericRepository<Manager>>();

        Console.WriteLine($"Service Type: {employeeService.GetType().Name} - HashCode: {employeeService.GetHashCode()}");

        while (true) //Use control flow statements (if, switch, for, foreach) to navigate a simple text-based menu.
        {
            Console.WriteLine("\n--- Employee Management System ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View All Employees");
            Console.WriteLine("3. Search Employee by ID");
            Console.WriteLine("4. Serialize Employees to JSON");
            Console.WriteLine("5. Get All Employees by Department"); //Linq querys
            Console.WriteLine("6. Get All Employees by Age Range");
            Console.WriteLine("7. Get Employee Basic Info"); // Using Linq projections
            Console.WriteLine("8. Get Employee Contact Info");
            Console.WriteLine("9. Get Employee Names");
            Console.WriteLine("10. Get Sum of Salaries Paid");// Using Linq Aggregation
            Console.WriteLine("11. Get Average of Age of Employees");// Using Linq Aggregation
            Console.WriteLine("12. Calculate Average Age (Extension Method)");
            Console.WriteLine("13. Test Service Lifetime");
            Console.WriteLine("14. Exit");
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

                    Console.Write("Enter Salary: ");
                    int sal = int.Parse(Console.ReadLine());

                    Console.Write("Enter Project Name: ");
                    string projectName = Console.ReadLine();

                    string departmentId = Guid.NewGuid().ToString();

                    if (isManager)
                    {
                        Console.Write("Enter Team Size: ");
                        int teamSize = int.Parse(Console.ReadLine());

                        var mgr = new Manager
                        {
                            Id = id,
                            Name = name,
                            Age = age,
                            Salary = sal,
                            TeamSize = teamSize
                        };

                        if (!string.IsNullOrEmpty(dept))
                        {
                            mgr.Department = new Department { Id = departmentId, Name = dept };
                            mgr.DepartmentId = departmentId;
                        }
                        if (!string.IsNullOrEmpty(projectName))
                        {
                            var mgrProject = await projectService.GetOrCreateProjectAsync(projectName);
                            mgr.Projects.Add(mgrProject);
                        }
                        await employeeService.AddEmployee(mgr);
                        managerRepo.Add(mgr);
                    }
                    else
                    {
                        var emp = new Employee
                        {
                            Id = id,
                            Name = name,
                            Age = age,
                            Salary = sal,
                        };
                        if (!string.IsNullOrEmpty(dept))
                        {
                            emp.Department = new Department { Id = departmentId, Name = dept };
                            emp.DepartmentId = departmentId;
                        }
                        if (!string.IsNullOrEmpty(projectName))
                        {
                            //check if project exists
                            var employeeProject = await projectService.GetOrCreateProjectAsync(projectName);
                            emp.Projects.Add(employeeProject);
                        }
                        await employeeService.AddEmployee(emp);
                        employeeRepo.Add(emp);
                    }
                    break;

                // case "19":
                //     //add project to employee
                //     Console.Write("Enter Employee ID: ");
                //     string empId = Console.ReadLine();
                //     Console.Write("Enter Project Name: ");
                //     string project = Console.ReadLine();
                //     await employeeService.AddProjectToEmployee(empId, project);
                //     break;
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
                    var allEmployees = await employeeService.GetAllEmployees();
                    var employeesByDept = allEmployees.FilterByDepartment(department);
                    if (!employeesByDept.Any())
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
                    var employeeBasicInfo = await employeeService.GetEmployeeBasicInfo();
                    if (employeeBasicInfo.Count == 0)
                    {
                        Console.WriteLine("No employee basic info found.");
                    }
                    else
                    {
                        foreach (var info in employeeBasicInfo)
                        {
                            Console.WriteLine($"Name: {info.Name}, Age: {info.Age}");
                        }
                    }
                    break; ;
                case "8":
                    var employeeContactInfo = await employeeService.GetEmployeeContactInfo();
                    if (employeeContactInfo.Count == 0)
                    {
                        Console.WriteLine("No employee  contact info found.");
                    }
                    else
                    {
                        foreach (var info in employeeContactInfo)
                        {
                            Console.WriteLine($"Name: {info.FullName}, Department: {info.DepartmentInfo}");
                        }
                    }
                    break;
                case "9":
                    var employeeNames = await employeeService.GetEmployeeNames();
                    if (employeeNames.Count == 0)
                    {
                        Console.WriteLine("No employee info found.");
                    }
                    else
                    {
                        foreach (var empName in employeeNames)
                        {
                            Console.WriteLine($"Name: {empName}");
                        }
                    }
                    break;
                case "10":
                    var sumSalary = await employeeService.GetSummaryofSalaries();
                    Console.WriteLine($"Total Salary Paid {sumSalary}");
                    break;
                case "11":
                    var avgAge = await employeeService.GetAverageAgeofEmployees();
                    Console.WriteLine($"Average Age of employees{avgAge}");
                    break;
                case "12":
                    var employees = await employeeService.GetAllEmployees();
                    if (employees.Any())
                    {
                        double averageAge = employees.CalculateAverageAge();
                        Console.WriteLine($"Average age of all employees: {averageAge:F2}");
                    }
                    else
                    {
                        Console.WriteLine("No employees found to calculate average age.");
                    }
                    break;
                case "13":
                    // Test service lifetime - get service multiple times
                    var service1 = host.Services.GetRequiredService<IEmployee>();
                    var service2 = host.Services.GetRequiredService<IEmployee>();

                    Console.WriteLine($"Service 1 HashCode: {service1.GetHashCode()}");
                    Console.WriteLine($"Service 2 HashCode: {service2.GetHashCode()}");
                    Console.WriteLine($"Are they the same instance? {ReferenceEquals(service1, service2)}");
                    break;
                case "14":
                    Console.WriteLine("Exiting the application.");
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
        foreach (var emp in employees)
        {
            Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}, Salary: {emp.Salary:C}, Department: {emp.Department?.Name ?? "No Department"}");
            emp.DisplayDetails();
        }
        var settings = new JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string json = JsonConvert.SerializeObject(employees, settings);
        Console.WriteLine(json);
    }
}