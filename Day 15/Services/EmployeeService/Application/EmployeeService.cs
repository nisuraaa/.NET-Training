public class EmployeeService : IEmployeeAppService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentHttpClient _departmentHttpClient;
    private readonly IProjectHttpClient _projectHttpClient;

    public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentHttpClient departmentHttpClient, IProjectHttpClient projectHttpClient)
    {
        _employeeRepository = employeeRepository;
        _departmentHttpClient = departmentHttpClient;
        _projectHttpClient = projectHttpClient;
    }
    public async Task<Employee> AddEmployee(CreateEmployeeRequest employeeRequest)
    {

        Employee employee;

        if (string.Equals(employeeRequest.isManager, "true", StringComparison.OrdinalIgnoreCase))
        {
            var mgr = new Manager
            {
                TeamSize = employeeRequest.teamCount ?? 0
            };
            mgr.Initialize(Guid.NewGuid().ToString(), employeeRequest.Name, employeeRequest.Age, employeeRequest.Salary);
            employee = mgr;
        }
        else
        {
            employee = new Employee();
            employee.Initialize(Guid.NewGuid().ToString(), employeeRequest.Name, employeeRequest.Age, employeeRequest.Salary);
        }

        if (employeeRequest.DepartmentName != null)
        {
            var existingDept = await _departmentHttpClient.GetDepartmentByNameAsync(employeeRequest.DepartmentName);
            if (existingDept == null)
            {
                var status = await _departmentHttpClient.CreateDepartmentAsync(employeeRequest.DepartmentName);
                if (status != null)
                {
                    employee.AssignDepartment(status.id);
                }
            }
            else
            {
                employee.AssignDepartment(existingDept.id); // Use existing department ID
            }
        }
        if (employeeRequest.ProjectName != null)
        {
            var existingProject = await _projectHttpClient.GetProjectByNameAsync(employeeRequest.ProjectName);
            if (existingProject == null)
            {
                var projectResponse = await _projectHttpClient.CreateProjectAsync(employeeRequest.ProjectName);
                if (projectResponse != null)
                {
                    employee.AddProject(projectResponse.id);
                }
            }
            else
            {
                employee.AddProject(existingProject.id);
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

        existingEmployee.SetName(request.Name);
        existingEmployee.SetAge(request.Age);
        existingEmployee.SetSalary(request.Salary);

        if (!string.IsNullOrEmpty(request.DepartmentName))
        {
            var department = await _departmentHttpClient.GetDepartmentByNameAsync(request.DepartmentName);
            if (department != null)
            {
                existingEmployee.AssignDepartment(department.id);
            }
            else
            {
                var created = await _departmentHttpClient.CreateDepartmentAsync(request.DepartmentName);
                if (created != null)
                {
                    existingEmployee.AssignDepartment(created.id);
                }
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
            Department = emp.DepartmentId != null ? emp.DepartmentId : "No Department"
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
            DepartmentInfo = emp.DepartmentId != null ? $"{emp.DepartmentId} (ID: {emp.DepartmentId})" : "No Department"
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
