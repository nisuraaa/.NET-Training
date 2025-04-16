//Create a new C# Console Application.

using Newtonsoft.Json;

class Employee
{
    //Define an Employee class with properties (Id, Name, Age, Department).

    public string Id;
    public string Name;
    public int Age;
    public string Department;
}

class Program
{
    static List<Employee> employeeList = new List<Employee>(); //Store employee data in a List<Employee> for easy retrieval and manipulation.
    static Dictionary<string, Employee> employeeDict = new Dictionary<string, Employee>(); //Use a Dictionary<int, Employee> to store employees with unique IDs.

    static void Main(string[] args)
    {

        while (true) //Use control flow statements (if, switch, for, foreach) to navigate a simple text-based menu.
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
                    AddEmployee();
                    break;
                case "2":
                    ViewEmployees();
                    break;
                case "3":
                    SearchEmployee();
                    break;
                case "4":
                    SerializeEmployees();
                    break;
                case "5":
                    return; // exit the app
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

    }

    static void AddEmployee()
    {
        Employee employee = new Employee();

        Console.WriteLine("Employee ID:");
        String Id = Console.ReadLine();
        employee.Id = Id;

        Console.WriteLine("Employee Name:");
        String Name = Console.ReadLine();
        if (!string.IsNullOrEmpty(Name))
        {
            employee.Name = char.ToUpper(Name[0]) + Name.Substring(1);
        }

        Console.WriteLine("Employee Age:");
        int Age = Int32.Parse(Console.ReadLine()); //Type Conversions
        while (true)
        {
            if (Age <= 0) //Implement user input validation (e.g., prevent negative ages).
            {
                Console.Write($"Invalid Age. Enter again: ");
                Age = Int32.Parse(Console.ReadLine()); //Type Conversion
            }
            else
            {
                break;
            }
        }
        employee.Age = Age;

        Console.WriteLine("Employee Department:");
        string Dept = Console.ReadLine();
        employee.Department = Dept;


        employeeList.Add(employee);
        employeeDict[Id] = employee;
        Console.WriteLine($"Employee {employee.Id} successfully created");
    }

    static void ViewEmployees()
    {
        foreach( var emp in employeeList)
        {
            Console.WriteLine($"{emp.Id}: {emp.Name}, {emp.Age} yrs, Dept: {emp.Department}");
        }
    }

    static void SearchEmployee()
    {
        Console.WriteLine("Enter ID to search:");
        string id = Console.ReadLine();

        if(employeeDict.TryGetValue(id, out Employee emp)){
            Console.WriteLine($"Found: {emp.Name}, Age: {emp.Age}, Dept: {emp.Department}");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void SerializeEmployees()
    {
        string json = JsonConvert.SerializeObject(employeeDict, Formatting.Indented);
        Console.WriteLine(json);
    }
}