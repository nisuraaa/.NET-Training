using System.Text;
using System.Text.Json;


public class ProjectHttpClient : IProjectHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProjectHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<CreateProjectResponse?> CreateProjectAsync(string projectName)
    {
        var request = new { Name = projectName };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/project", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreateProjectResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }

    public async Task<GetProjectResponse?> GetProjectByNameAsync(string name)
    {
        var response = await _httpClient.GetAsync($"/api/project/name/{name}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetProjectResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }

    public async Task<GetProjectResponse?> GetProjectByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"/api/project/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetProjectResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return null;
    }
}
