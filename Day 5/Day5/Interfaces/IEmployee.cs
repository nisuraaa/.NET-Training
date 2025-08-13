public interface IEmployee
{
    Task AddEmployee(Employee employee);
    Task<Employee> GetEmployeeByIdAsync(string id);
    Task<List<Employee>> GetAllEmployees();
    Task<List<Employee>> GetAllEmployeesbyDepartment(string department);
    Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit);

    Task<List<EmployeeSummary>> GetEmployeeSummaries();
    Task<List<EmployeeBasicInfo>> GetEmployeeBasicInfo();
    Task<List<EmployeeContactInfo>> GetEmployeeContactInfo();
    Task<List<DepartmentSummary>> GetDepartmentSummaries();
    Task<List<ManagerSummary>> GetManagerSummaries();
    Task<List<string>> GetEmployeeNames();
    Task<List<string>> GetDepartmentNames();
}

public class EmployeeSummary
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
}

public class EmployeeBasicInfo
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class EmployeeContactInfo
{
    public string FullName { get; set; }
    public string DepartmentInfo { get; set; }
}

public class DepartmentSummary
{
    public string Department { get; set; }
    public int EmployeeCount { get; set; }
    public double AverageAge { get; set; }
    public List<string> EmployeeNames { get; set; }
}

public class ManagerSummary
{
    public string Name { get; set; }
    public string Department { get; set; }
    public int TeamSize { get; set; }
    public string ManagerInfo { get; set; }
}