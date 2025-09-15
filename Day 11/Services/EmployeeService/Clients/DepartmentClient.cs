using System.Text;
using System.Text.Json;


public class DepartmentHttpClient : IDepartmentHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public DepartmentHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Department?> CreateDepartmentAsync(string departmentName)
    {
        var request = new { Name = departmentName };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/department", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Department>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }

    public async Task<Department?> GetDepartmentByNameAsync(string name)
    {
        var response = await _httpClient.GetAsync($"/api/department/name/{name}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Department>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }

    public async Task<Department?> GetDepartmentByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"/api/department/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Department>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }
}
