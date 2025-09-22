public interface IEmployeeRepository
{
    // Basic CRUD operations
    Task<Employee?> GetByIdAsync(string id);
    Task<List<Employee>> GetAllAsync();
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(string id);
}