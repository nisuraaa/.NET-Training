using Microsoft.EntityFrameworkCore;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly dbContext _dbContext;

    public DepartmentRepository(dbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        return await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task<Department?> GetByIdAsync(string id)
    {
        return await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _dbContext.Departments
            .ToListAsync();
    }

    public async Task<Department> AddAsync(Department department)
    {
        _dbContext.Departments.Add(department);
        await _dbContext.SaveChangesAsync();
        return department;
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();
        return department;
    }

    public async Task<bool> DeleteAsync(string name)
    {
        var department = await GetByNameAsync(name);
        if (department == null)
        {
            return false;
        }

        _dbContext.Departments.Remove(department);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}