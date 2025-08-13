using Microsoft.EntityFrameworkCore;

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
    public async Task AddEmployee(Employee employee)
    {

        // using var context = new dbContext();

        // Ensure department exists or create it
        if (employee.Department != null)
        {
            var existingDept = await _departmentRepository.GetByNameAsync(employee.Department.Name);
            if (existingDept == null)
            {
                employee.Department.Id = Guid.NewGuid().ToString();
                await _departmentRepository.AddAsync(employee.Department);
            }
            else
            {
                employee.Department.Id = existingDept.Id; // Use existing department ID
                employee.Department = null; // Avoid adding duplicate
            }
        }
        // Handle projects - attach existing projects to avoid duplicates

        if (employee.Projects != null || employee.Projects.Count != 0)
        {
            var projectsToAttach = new List<Project>();
            foreach (var project in employee.Projects.ToList())
            {
                var existingProject = await _projectRepository.GetByIdAsync(project.Id);
                if (existingProject != null)
                {
                    projectsToAttach.Add(existingProject);
                }
                else
                {
                    projectsToAttach.Add(project);
                    await _projectRepository.AddAsync(project);
                }
            }
            employee.Projects.Clear();
            foreach (var project in projectsToAttach)
            {
                employee.Projects.Add(project);
            }

        }

        await _employeeRepository.AddAsync(employee);
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