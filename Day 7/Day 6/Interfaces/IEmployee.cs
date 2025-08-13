public interface IEmployee
{
    Task AddEmployee(Employee employee);
    Task<Employee?> GetEmployeeByIdAsync(string id);
    Task<List<Employee>> GetAllEmployees();
    Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit);

    Task<List<EmployeeSummary>> GetEmployeeSummaries();
    Task<List<EmployeeBasicInfo>> GetEmployeeBasicInfo();
    Task<List<EmployeeContactInfo>> GetEmployeeContactInfo();
    Task<List<string>> GetEmployeeNames();
    Task<List<string>> GetDepartmentNames();

    Task<decimal> GetSummaryofSalaries();
    Task<double> GetAverageAgeofEmployees();
    
    // Task AddProjectToEmployee(string employeeId, string projectName);
}
