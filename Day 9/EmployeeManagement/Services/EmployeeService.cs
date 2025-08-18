using System.Globalization;
using Microsoft.EntityFrameworkCore;
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
}
public class EmployeeService : IEmployee
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IProjectRepository _projectRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IProjectRepository projectRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _projectRepository = projectRepository;
    }
    public async Task<Employee> AddEmployee(CreateEmployeeRequest employeeRequest)
    {

        Employee employee;

        if (string.Equals(employeeRequest.isManager, "true", StringComparison.OrdinalIgnoreCase))
        {
            employee = new Manager
            {
                Id = Guid.NewGuid().ToString(),
                Name = employeeRequest.Name,
                Age = employeeRequest.Age,
                Salary = employeeRequest.Salary,
                TeamSize = employeeRequest.teamCount ?? 0
            };
        }
        else
        {
            employee = new Employee
            {
                Id = Guid.NewGuid().ToString(),
                Name = employeeRequest.Name,
                Age = employeeRequest.Age,
                Salary = employeeRequest.Salary,
            };
        }

        if (employeeRequest.DepartmentName != null)
        {
            var existingDept = await _departmentRepository.GetByNameAsync(employeeRequest.DepartmentName);
            if (existingDept == null)
            {
                var newDept = new Department
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = employeeRequest.DepartmentName
                };
                await _departmentRepository.AddAsync(newDept);
                employee.DepartmentId = newDept.Id;
            }
            else
            {
                employee.DepartmentId = existingDept.Id; // Use existing department ID
            }
        }

        if (employeeRequest.ProjectName != null)
        {
            var projectsToAttach = new List<Project>();
            var existingProject = await _projectRepository.GetByNameAsync(employeeRequest.ProjectName);
            if (existingProject != null)
            {
                projectsToAttach.Add(existingProject);
            }
            employee.Projects.Clear();
            foreach (var project in projectsToAttach)
            {
                employee.Projects.Add(project);
            }

        }

        return await _employeeRepository.AddAsync(employee);
    }

    public async Task<Employee> UpdateEmployee(UpdateEmployeeRequest request)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(request.Id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {request.Id} not found");
        }

        existingEmployee.Name = request.Name;
        existingEmployee.Age = request.Age;
        existingEmployee.Salary = request.Salary;

        if (!string.IsNullOrEmpty(request.DepartmentName))
        {
            var department = await _departmentRepository.GetByNameAsync(request.DepartmentName);
            if (department != null)
            {
                existingEmployee.DepartmentId = department.Id;
                existingEmployee.Department = department;
            }
            else
            {
                // If department does not exist, create a new one
                department = new Department { Id = Guid.NewGuid().ToString(), Name = request.DepartmentName };
                await _departmentRepository.AddAsync(department);
                existingEmployee.DepartmentId = department.Id;
                existingEmployee.Department = department;

            }
        }

        return await _employeeRepository.UpdateAsync(existingEmployee);
    }

    public async Task<Employee?> GetEmployeeByIdAsync(string id)
    {
        return await _employeeRepository.GetByIdAsync(id);
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        return await _employeeRepository.GetAllAsync();
    }

    public async Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit)
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Where(e => e.Age >= lowerAgeLimit && e.Age <= upperAgeLimit).ToList();
    }

    public async Task<List<EmployeeSummary>> GetEmployeeSummaries()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(emp => new EmployeeSummary
        {
            Id = emp.Id,
            Name = emp.Name,
            Department = emp.Department != null ? emp.Department.Name : "No Department"
        }).ToList();
    }

    public async Task<List<EmployeeBasicInfo>> GetEmployeeBasicInfo()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(emp => new EmployeeBasicInfo
        {
            Name = emp.Name,
            Age = emp.Age,
        }).ToList();
    }

    public async Task<List<EmployeeContactInfo>> GetEmployeeContactInfo()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(emp => new EmployeeContactInfo
        {
            FullName = emp.Name,
            DepartmentInfo = emp.Department != null ? $"{emp.Department.Name} (ID: {emp.Department.Id})" : "No Department"
        }).OrderBy(info => info.FullName).ToList();
    }

    public async Task DeleteEmployee(string id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {id} not found");
        }

        await _employeeRepository.DeleteAsync(employee.Id);
    }

    public async Task<List<string>> GetDepartmentNames()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(d => d.Name).ToList();
    }
    public async Task<List<string>> GetEmployeeNames()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(e => e.Name).ToList();
    }
    public async Task<decimal> GetSummaryofSalaries()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Sum(e => e.Salary);
    }
    public async Task<double> GetAverageAgeofEmployees()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Average(e => (double)e.Age);
    }

}