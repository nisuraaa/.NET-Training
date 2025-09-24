    public interface IDepartmentHttpClient
    {
        Task<CreateDepartmentResponse?> CreateDepartmentAsync(string departmentName);
        Task<GetDepartmentResponse?> GetDepartmentByNameAsync(string name);
        Task<GetDepartmentResponse?> GetDepartmentByIdAsync(string id);

    }
