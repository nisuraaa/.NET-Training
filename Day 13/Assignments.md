Category Day Estimated Time Topics to follow Status of Completion Task

Basic Fundamentals Day 1: C# Fundamentals 8 hours (Buffer +8h)

```
C# Syntax
Data Types , Converting Types, String Handling
Control Flows, Loops, Conditions,
Lists, Dictionaries, Collections
NuGet Package Management
```
```
Completed
```
```
Task: Create a simple console app that uses collections, loops, and exception handling.
1⃣ Create a new C# Console Application.
2⃣ Define an Employee class with properties (Id, Name, Age, Department).
3⃣ Implement basic data types (int, string, bool, double, etc.) and practice type conversions.
4⃣ Use string handling to format employee names (e.g., capitalize first letters).
5⃣ Implement user input validation (e.g., prevent negative ages).
6⃣ Use control flow statements (if, switch, for, foreach) to navigate a simple text-based menu.
7⃣ Store employee data in a List<Employee> for easy retrieval and manipulation.
8⃣ Use a Dictionary<int, Employee> to store employees with unique IDs.
9⃣ Manage dependencies with NuGet, e.g., install Newtonsoft.Json for JSON serialization.
```
Basic Fundamentals Day 2: Object-Oriented Programming (OOP) in C# 6-8 hours

```
Interfaces & Abstract Classes
Encapsulation, Polymorphism, and Inheritance
Generic Types in C#
```
```
Completed
```
```
Task: Implement interfaces and generics in a simple program.
1⃣ Create an IEmployeeService interface defining methods like AddEmployee, GetEmployeeById, and GetAllEmployees.
2⃣ Implement the interface in an EmployeeService class to handle employee-related logic.
3⃣ Use encapsulation by making fields private and exposing only necessary properties.
4⃣ Apply inheritance by creating a Manager class that inherits from Employee and adds a TeamSize property.
5⃣ Implement polymorphism by overriding a DisplayDetails() method in both Employee and Manager.
6⃣ Use generics to create a GenericRepository<T> class for storing different entity types.
```
Basic Fundamentals Day 3: Debugging & Unit Testing in .NET 6-8 hours

```
Debugging in Visual Studio
Breakpoints, Watch, and Immediate Window
Unit Testing with xUnit & Moq
```
```
Completed
```
```
Task: Write unit tests using xUnit for basic functions.
1⃣ Set breakpoints and use the Watch window to inspect variables.
2⃣ Use the Immediate Window to test expressions while debugging.
3⃣ Write unit tests using xUnit to verify EmployeeService methods.
4⃣ Mock dependencies using Moq to test service methods in isolation.
```
Basic Fundamentals Day 4: Exception Handling & Logging with Serilog 6-8 hours

```
Global Exception Handling in .NET
Throw Custom Exceptions
Logging with Serilog & ILogger
```
```
Completed
```
```
Task: Implement Serilog structured logging in a Web API/ Console app.
1⃣ Implement try-catch blocks to handle invalid user input.
2⃣ Create custom exceptions (e.g., EmployeeNotFoundException).
3⃣ Implement global exception handling in a centralized location.
4⃣ Use Serilog for structured logging (log errors to file and console).
```
Basic Fundamentals Day 5: Async Programming & LINQ in C# 6-8 hours
**Async/Await** in .NET
**LINQ** Queries (Filtering, Aggregation, Projections)
Completed

```
Task: Implement LINQ queries and async operations in C#.
1⃣ Convert methods to use async/await to simulate database calls.
2⃣ Use LINQ queries to filter employees by age, department, etc.
3⃣ Implement LINQ projections to display only specific properties of employees.
4⃣ Use LINQ aggregate functions (e.g., Average, Max, Count).
```
Basic Fundamentals Day 6: Extension Methods & Introduction to Dependency Injection (DI) 6-8 hours

```
Extension Methods in C#
Dependency Injection in .NET Core
Service Lifetimes ( Transient, Scoped, Singleton )
```
```
Completed
```
```
Task: Implement custom extension methods & DI in a console app. Use DI registered classes.
1⃣ Implement Dependency Injection by registering IEmployeeService in a DI container.
2⃣ Use different service lifetimes (Singleton, Scoped, Transient) and observe their behavior.
3⃣ Create extension methods for:
Formatting employee names (e.g., "john doe" → "John Doe").
Filtering employees by department.
Calculating the average employee age.
```
Basic Fundamentals Day 7: Entity Framework Core – Code First Approach 6-8 hours (Buffer +8h)

```
EF Core Model vs Entity Mapping
FluentAPI with EF core
Code First Approach (Migrations, DbContext, DbSets)
EF Core Relationships (One-to-Many, Many-to-Many)
```
```
In progress
```
```
Task: Implement Console app with EF Core using Code-First approach and SQL Server.
1⃣ Set up Entity Framework Core and configure a database connection.
2⃣ Define the Employee entity and map it to a table.
3⃣ Use Fluent API to define constraints (e.g., Name max length 50).
4⃣ Create database migrations and apply them.
5⃣ Implement one-to-many relationships (e.g., Department → Employees).
6⃣ Implement many-to-many relationships (e.g., Employees ↔ Projects).
```
Basic Fundamentals Day 8: Repository Pattern with EF Core + FluentAPI 4-8 hours
**Repository** Pattern in .NET
Writing advanced **LINQ and Lambda** Queries

```
Task: Implement a Repository in the API.
1⃣ Implement the Repository Pattern for Employee data retrieval.
2⃣ Write advanced LINQ queries, including joins, grouping, and nested queries.
```
Basic Enterprise Application Development Building Web APIs with ASP.NET Core 16 hours
Controllers, Routing, Middleware
**RESTful API** design in .NET

```
Task: Build a basic Web API with GET, POST, PUT, DELETE endpoints.
1⃣ Create a Web API project in .NET Core.
2⃣ Implement EmployeeController with CRUD endpoints.
3⃣ Use routing, middleware, and attribute-based validation.
4⃣ Return appropriate HTTP status codes (200, 404, 500).
```
Basic Enterprise Application Development Microservices Architecture 8-16 hours Monolith vs Microservices
**Microservice Architecture**^

```
1⃣ Refactor the application into separate microservices (e.g., EmployeeService, DepartmentService).
2⃣ Use API Gateway for centralized access.
3⃣ Deploy services as independent containers.
```
Basic Enterprise Application Development Domain-Driven Design (DDD) in Microservices 8-16 hours **DDD Concepts** (Entities, Aggregates, Value Objects)
Implementing Domain Layers in .NET^

```
Task: Apply DDD principles to the microservices.
1⃣ Define Entities, Aggregates, and Value Objects.
2⃣ Use domain layers (Application, Domain, Infrastructure, Persistence).
3⃣ Ensure domain logic is encapsulated in Aggregate Roots.
```
Basic Enterprise Application Development CQRS & Mediator Implementation + Validations 8-16 hours

```
CQRS Overview (Separating Read & Write Models)
Implementing MediatR in .NET
Fluent Validations
```
```
Task: Apply CQRS with MediatR in the API.
1⃣ Implement the CQRS pattern.
2⃣ Use MediatR to handle commands and queries.
3⃣ Apply Fluent Validation for input validation.
```
Basic Enterprise Application Development Event-Driven Architecture with RabbitMQ/MassTransit 16-24 hours
Message Queues & Pub/Sub Model
**MassTransit with RabbitMQ** in .Net

```
Task: Implement event-driven communication using RabbitMQ.
1⃣ Use RabbitMQ for messaging.
2⃣ Publish and consume events using MassTransit.
3⃣ Handle event-driven updates between microservices.
```
Basic Enterprise Application Development Caching with Redis & API Performance Optimization 16-24 hours

```
Caching Strategies (Memory, Distributed, Redis)
Improving API Performance (Asynchronous Queries, Lazy
Loading, Indexing)
```
```
Task: Implement Redis caching for API responses.
1⃣ Implement caching strategies (Memory, Distributed, Redis).
2⃣ Optimize API performance using asynchronous queries.
3⃣ Enable lazy loading and indexing.
```
Basic Enterprise Application Development Securing Microservices & OAuth 2.0 8-16 hours
JWT & OAuth 2.0 **Authentication** in .Net
Role-Based Authorization in .Net

```
Task: Implement JWT authentication in microservices.
1⃣ Implement JWT-based authentication.
2⃣ Use OAuth 2.0 for token-based security.
3⃣ Apply Role-Based Authorization for different user roles.
```
Basic Enterprise Application Development Deployment 8-16 hours
**IIS Deployment**
Kubernetes , Docker

```
1⃣ Deploy the API to IIS.
2⃣ Containerize services using Docker.
3⃣ Orchestrate with Kubernetes.
```
Basic Enterprise Application Development Blazor in .NET 32-40 hours

```
Introduction to Blazor, Data binding, Event Handling,
Components, Routing, JS interop in Blazor, AUthentication,
Server pre rendering, SignalR
```
Basic Enterprise Application Development SignalR in .NET 24 hours Core Concepts of SignalR

```
1⃣ Integrate SignalR into the application.
2⃣ Send real-time notifications when new employees are added.
3⃣ Broadcast changes to all connected clients.
```
Resource Reference link

Get started with Visual Basic https://learn.microsoft.com/en-us/dotnet/visual-basic/getting-started/

A tour of the C# language
https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-
csharp/overview

Create a .NET console application
https://learn.microsoft.com/en-us/dotnet/core/tutorials/with-visual-
studio

Tutorials for getting started with .NET https://learn.microsoft.com/en-us/dotnet/standard/get-started

Create a web API with ASP.NET Core
https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?
view=aspnetcore-9.0&tabs=visual-studio

microsoft/learn videos https://dotnet.microsoft.com/en-us/learn/videos

Prerequisites Guide

Microsoft Visual Studio Community 2022 - v17.0 or
later

https://visualstudio.microsoft.com/downloads/

*When installing VS2022 make sure to select **ASP.NET and web
developement, Azure development, .NET desktop development** under
workloads and **.NET 8.0 Runtime, .NET 9.0 Runtime, .NET SDK** other
than the default selected values under Individual components
Microsoft SQL Server 2022 (Developer edition) - v16.
0 or later

https://www.microsoft.com/en-us/sql-server/sql-
server-downloads

```
When installing sql server make sure to select Mixed Mode
Authentication (SQL Server and Windows Authentication mode) during
setup
```
SQL Server Management Studio v20.0 or later

https://learn.microsoft.com/en-us/ssms/download-
sql-server-management-studio-ssms#download-
ssms

```
Default installation
```

