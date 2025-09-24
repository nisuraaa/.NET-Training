using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;
using MassTransit;

// Application Layer
using ProjectService.Application;
using ProjectService.Application.EventHandlers;

// Domain Layer
using ProjectService.Domain.Repositories;

// Infrastructure Layer
using ProjectService.Infrastructure.Persistence;
using ProjectService.Infrastructure.Repository;
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

// Register DbContext with SQLite
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlite("Data Source=project.db"));

// Register repositories and services
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectAppService, ProjectService.Application.ProjectService>();

// Add JWT Authentication and Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmployeeEventConsumer>();
    x.AddConsumer<DepartmentEventConsumer>();
    
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
    var context = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    await context.Database.EnsureCreatedAsync();
}


app.Run();
