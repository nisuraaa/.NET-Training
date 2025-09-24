
using DepartmentServices.Interfaces;

namespace DepartmentServices.Services
{
    public class DepartmentService : IDepartmentAppService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task<Department?> GetByIdAsync(string id)
        {
            return _departmentRepository.GetByIdAsync(id);
        }

        public Task<Department?> GetByNameAsync(string name)
        {
            return _departmentRepository.GetByNameAsync(name);
        }

        public Task<IEnumerable<Department>> GetAllAsync()
        {
            return _departmentRepository.GetAllAsync();
        }

        public Task<Department> AddAsync(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
            {
                throw new ArgumentException("Department name cannot be null or empty.", nameof(departmentName));
            }
            var department = new Department();
            department.Initialize(Guid.NewGuid().ToString(), departmentName);
            return _departmentRepository.AddAsync(department);
        }

        public Task<Department> UpdateAsync(Department department)
        {
            return _departmentRepository.UpdateAsync(department);
        }

        public Task<bool> DeleteAsync(string id)
        {
            return _departmentRepository.DeleteAsync(id);
        }
    }
}
