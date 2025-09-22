public interface IDepartmentAppService
{
    Task<Department> AddAsync(string name);
    Task<Department?> GetByIdAsync(string id);
    Task<Department?> GetByNameAsync(string name);
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department> UpdateAsync(Department department);
    Task<bool> DeleteAsync(string id);
}
