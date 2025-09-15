    public interface IDepartmentHttpClient
    {
        Task<Department?> CreateDepartmentAsync(string departmentName);
        Task<Department?> GetDepartmentByNameAsync(string name);
        Task<Department?> GetDepartmentByIdAsync(string id);

    }
