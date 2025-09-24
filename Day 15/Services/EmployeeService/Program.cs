
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using EmployeeServices.Application;
using MassTransit;
using EmployeeServices.Application.EventHandlers;
using SharedEvents.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlite("Data Source=employee.db"));

// Register repositories and services
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();

// Add JWT Authentication and Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DepartmentEventConsumer>();
    x.AddConsumer<ProjectEventConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHttpClient<IDepartmentHttpClient, DepartmentHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5106"); // Department service URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient<IProjectHttpClient, ProjectHttpClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5043"); // Project service URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.Run();
