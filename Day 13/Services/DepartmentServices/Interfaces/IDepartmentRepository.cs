public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(string id);
    Task<Department?> GetByNameAsync(string name);
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department> AddAsync(Department department);
    Task<Department> UpdateAsync(Department department);
    Task<bool> DeleteAsync(string id);
}