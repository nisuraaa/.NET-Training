public class EmployeeService : IEmployee
{
    private readonly Dictionary<string, Employee> _employees = new();

    public async Task AddEmployee(Employee emp)
    {
        await Task.Delay(3000); // Simulate async operation
        if (!_employees.ContainsKey(emp.Id))
            _employees[emp.Id] = emp;
    }

    public async Task<Employee> GetEmployeeByIdAsync(string id)
    {
        await Task.Delay(3000); // Simulate async operation
        return _employees.TryGetValue(id, out var emp) ? emp : null;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        await Task.Delay(3000); // Simulate async operation
        return new List<Employee>(_employees.Values);
    }

    public async Task<List<Employee>> GetAllEmployeesbyDepartment(string department)
    {
        //LINQ query to filter employees by department.
        await Task.Delay(3000); // Simulate async operation
        return _employees.Values
            .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit)
    {
        await Task.Delay(3000);
        return _employees.Values
            .Where(e => e.Age >= lowerAgeLimit && e.Age <= upperAgeLimit)
            .ToList();
    }

    
}