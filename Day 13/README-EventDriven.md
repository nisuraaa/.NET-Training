# Event-Driven Microservices with RabbitMQ

This project implements event-driven communication between microservices using RabbitMQ and MassTransit.

## Architecture

The system consists of three microservices:
- **DepartmentService** - Manages departments
- **EmployeeService** - Manages employees
- **ProjectService** - Manages projects

Each service publishes events when data changes and consumes events from other services.

## Event Flow

### Department Events
- `DepartmentCreatedEvent` - Published when a department is created
- `DepartmentUpdatedEvent` - Published when a department is updated
- `DepartmentDeletedEvent` - Published when a department is deleted

### Employee Events
- `EmployeeCreatedEvent` - Published when an employee is created
- `EmployeeUpdatedEvent` - Published when an employee is updated
- `EmployeeDeletedEvent` - Published when an employee is deleted

### Project Events
- `ProjectCreatedEvent` - Published when a project is created
- `ProjectUpdatedEvent` - Published when a project is updated
- `ProjectDeletedEvent` - Published when a project is deleted

## Prerequisites

1. .NET 9.0 SDK
2. Docker and Docker Compose
3. RabbitMQ (via Docker)

## Setup Instructions

### 1. Start RabbitMQ

```bash
# Start RabbitMQ using Docker Compose
docker-compose up -d

# Verify RabbitMQ is running
docker ps
```

RabbitMQ Management UI will be available at: http://localhost:15672
- Username: guest
- Password: guest

### 2. Build the Solution

```bash
# Build all projects
dotnet build Microservices.sln
```

### 3. Run the Services

Open three terminal windows and run each service:

**Terminal 1 - Department Service:**
```bash
cd Services/DepartmentServices
dotnet run
```

**Terminal 2 - Employee Service:**
```bash
cd Services/EmployeeService
dotnet run
```

**Terminal 3 - Project Service:**
```bash
cd Services/ProjectService
dotnet run
```

## Testing Event-Driven Communication

### 1. Create a Department
```bash
curl -X POST "https://localhost:5106/api/Department" \
  -H "Content-Type: application/json" \
  -d '{"name": "Engineering"}'
```

### 2. Create an Employee
```bash
curl -X POST "https://localhost:5043/api/Employee" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "age": 30,
    "salary": 75000,
    "departmentName": "Engineering",
    "isManager": "false"
  }'
```

### 3. Create a Project
```bash
curl -X POST "https://localhost:5043/api/Project" \
  -H "Content-Type: application/json" \
  -d '{"name": "Microservices Migration"}'
```

## Monitoring Events

1. **RabbitMQ Management UI**: Visit http://localhost:15672 to see queues and message flow
2. **Service Logs**: Check the console output of each service to see event consumption logs
3. **Swagger UI**: Each service has Swagger documentation available at:
   - Department Service: https://localhost:5106/swagger
   - Employee Service: https://localhost:5043/swagger
   - Project Service: https://localhost:5043/swagger

## Event Handlers

Each service includes event handlers that log received events:

- **DepartmentService**: Consumes Employee and Project events
- **EmployeeService**: Consumes Department and Project events
- **ProjectService**: Consumes Employee and Department events

## Troubleshooting

### RabbitMQ Connection Issues
- Ensure RabbitMQ is running: `docker ps | grep rabbitmq`
- Check RabbitMQ logs: `docker logs microservices-rabbitmq`
- Verify connection settings in Program.cs files

### Service Startup Issues
- Ensure all services are built: `dotnet build`
- Check for port conflicts (5106, 5043, 5043)
- Verify database files are created (department.db, employee.db, project.db)

### Event Not Received
- Check RabbitMQ Management UI for message queues
- Verify event handlers are registered in Program.cs
- Check service logs for error messages

## Next Steps

This implementation provides a foundation for event-driven microservices. You can extend it by:

1. Adding more sophisticated event handling logic
2. Implementing event sourcing
3. Adding retry policies and dead letter queues
4. Implementing saga patterns for distributed transactions
5. Adding monitoring and observability tools
