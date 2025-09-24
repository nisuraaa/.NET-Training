using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _dbContext;

    public EmployeeRepository(EmployeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _dbContext.Employees
            .ToListAsync();
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _dbContext.Employees.Update(employee);
        await _dbContext.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee == null) return false;

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}