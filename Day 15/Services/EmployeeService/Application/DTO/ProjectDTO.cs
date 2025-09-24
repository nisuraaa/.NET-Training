public class CreateProjectRequest
{
    public required string Name { get; set; } = string.Empty;
}

public class CreateProjectResponse
{
    public required string id {get; set;}
    public required string Name {get; set;}
}

public class GetProjectResponse
{
    public required string id {get; set;}
    public required string Name {get; set;}
}