namespace EmployeeServices.Domain.Repositories;

using EmployeeServices.Domain.Entities;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(string id);
    Task<List<Employee>> GetAllAsync();
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(string id);
}
