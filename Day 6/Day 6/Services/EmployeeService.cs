using Microsoft.EntityFrameworkCore;

public class EmployeeService : IEmployee
{
    private dbContext CreateContext()
    {
        return new dbContext(); // Creates new database connection
    }

    public async Task AddEmployee(Employee employee)
    {
        using var context = new dbContext();

        // Ensure department exists or create it
        if (employee.Department != null)
        {
            var existingDept = await context.Departments
                .FirstOrDefaultAsync(d => d.Name == employee.Department.Name);

            if (existingDept == null)
            {
                employee.Department.Id = Guid.NewGuid().ToString();
                context.Departments.Add(employee.Department);
            }
            else
            {
                employee.DepartmentId = existingDept.Id;
                employee.Department = null; // Avoid adding duplicate
            }
        }

        // Handle projects - attach existing projects to avoid duplicates
        if (employee.Projects.Any())
        {
            var projectsToAttach = new List<Project>();
            foreach (var project in employee.Projects.ToList())
            {
                var existingProject = await context.Projects
                    .FirstOrDefaultAsync(p => p.Id == project.Id);
                    
                if (existingProject != null)
                {
                    projectsToAttach.Add(existingProject);
                }
                else
                {
                    projectsToAttach.Add(project);
                    context.Projects.Add(project);
                }
            }
            employee.Projects.Clear();
            foreach (var project in projectsToAttach)
            {
                employee.Projects.Add(project);
            }
        }

        context.Employees.Add(employee);
        await context.SaveChangesAsync();
    }
    public async Task<Employee?> GetEmployeeByIdAsync(string id)
    {
        using var context = new dbContext();
        return await context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        using var context = new dbContext();
        return await context.Employees
            .Include(e => e.Department)
            .ToListAsync();
    }

    // public async Task<List<Employee>> GetAllEmployeesbyDepartment(string department)
    // {
    //     //LINQ query to filter employees by department.
    //     await Task.Delay(3000); // Simulate async operation
    //     return _employees.Values
    //         .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
    //         .ToList();
    // }

    public async Task<List<Employee>> GetAllEmployeesbyAgeRange(int lowerAgeLimit, int upperAgeLimit)
    {
        using var context = new dbContext();
        return await context.Employees
            .Include(e => e.Department)
            .Where(e => e.Age >= lowerAgeLimit && e.Age <= upperAgeLimit)
            .ToListAsync();
    }

    public async Task<List<EmployeeSummary>> GetEmployeeSummaries()
    {
        using var context = new dbContext();
        return await context.Employees
                    .Include(e => e.Department)
                    .Select(emp => new EmployeeSummary
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Department = emp.Department != null ? emp.Department.Name : "No Department"
                    })
                    .OrderBy(summary => summary.Name)
                    .ToListAsync();
    }

    public async Task<List<EmployeeBasicInfo>> GetEmployeeBasicInfo()
    {
        using var context = new dbContext();
        return await context.Employees
            .Select(e => new EmployeeBasicInfo { Name = e.Name, Age = e.Age })
            .ToListAsync();
    }

    public async Task<List<EmployeeContactInfo>> GetEmployeeContactInfo()
    {
        using var context = new dbContext();
        return await context.Employees
            .Include(e => e.Department)
            .Select(emp => new EmployeeContactInfo
            {
                FullName = emp.Name,
                DepartmentInfo = emp.Department != null ? $"{emp.Department.Name} (ID: {emp.Department.Id})" : "No Department"
            })
            .OrderBy(info => info.FullName)
            .ToListAsync();
    }

    public async Task<List<string>> GetDepartmentNames()
    {
        using var context = new dbContext();
        return await context.Departments
            .Select(d => d.Name)
            .ToListAsync();
    }
    public async Task<List<string>> GetEmployeeNames()
    {
        using var context = new dbContext();
        return await context.Employees
            .Select(e => e.Name)
            .ToListAsync();
    }
    public async Task<decimal> GetSummaryofSalaries()
    {
        using var context = new dbContext();
        return await context.Employees
            .SumAsync(e => e.Salary);
    }
    public async Task<double> GetAverageAgeofEmployees()
    {
        using var context = new dbContext();
        return await context.Employees
            .AverageAsync(e => (double)e.Age);
    }

    // public async Task AddProjectToEmployee(string employeeId, string projectName)
    // {
    //     using var context = new dbContext();
        
    //     var employee = await context.Employees
    //         .Include(e => e.Projects)
    //         .FirstOrDefaultAsync(e => e.Id == employeeId);
            
    //     if (employee == null)
    //     {
    //         throw new Exception("Employee not found");
    //     }
        
    //     // Get or create the project
    //     var project = await context.Projects
    //         .FirstOrDefaultAsync(p => p.Name.ToUpper() == projectName.ToUpper());
            
    //     if (project == null)
    //     {
    //         project = new Project
    //         {
    //             Id = Guid.NewGuid().ToString(),
    //             Name = projectName
    //         };
    //         context.Projects.Add(project);
    //     }
        
    //     // Check if employee already has this project
    //     if (!employee.Projects.Any(p => p.Id == project.Id))
    //     {
    //         employee.Projects.Add(project);
    //         await context.SaveChangesAsync();
    //     }
    //     else
    //     {
    //         throw new Exception("Project already assigned to employee");
    //     }
    // }



}