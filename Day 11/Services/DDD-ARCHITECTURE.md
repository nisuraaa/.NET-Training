# Domain-Driven Design (DDD) Refactor Plan for Microservices

This document describes what was changed to apply DDD across the three services in this solution and why. It also outlines a migration plan so the system keeps working while we refactor incrementally.

Services:
- DepartmentServices
- EmployeeService
- ProjectService

## Goals
- Encapsulate invariants and domain logic inside Aggregate Roots.
- Introduce clear layers per service: Application, Domain, Infrastructure, Persistence (EF Core).
- Keep APIs stable during the transition to avoid breaking clients.

---

## New Layered Structure (per service)

Each service will gradually align to the following structure:

- Application/
  - Commands, Queries, DTO mapping
  - Orchestrates use cases by calling Domain
- Domain/
  - Entities/ (Aggregate roots and entities)
  - ValueObjects/ (immutable types enforcing invariants)
  - Events/ (domain events, if needed)
  - Repositories/ (interfaces only)
- Infrastructure/
  - Implementations of domain services (e.g., HTTP clients, 3rd-party integrations)
- Persistence/
  - EF Core DbContext, Configurations, Repository implementations
- API layer (Controllers) remains thin and calls Application layer

We started by scaffolding the Domain layer (Entities + ValueObjects) for all three services. Next steps will move repository interfaces into Domain and EF details into Persistence.

---

## Bounded Contexts and Aggregates

### DepartmentServices
- Aggregate Root: `Department`
  - Identity: `Id` (string)
  - Invariants: `DepartmentName` value object (non-empty, <= 50 chars)
  - Behaviors: `Rename`
- Value Objects:
  - `DepartmentName`

Current status:
- Added `Domain/ValueObjects/DepartmentName.cs`
- Added `Domain/Entities/Department.cs` with factory `Create(name)` and `Rename(newName)`
- Existing EF entity in `Models/Department.cs` still used by current DbContext to avoid breaking runtime. We will migrate EF to use the Domain entity via Persistence layer in a follow-up step.

Next steps:
- Move `IDepartmentRepository` into `Domain/Repositories` and make it depend on Domain `Department`.
- Create `Persistence` folder with `DbContext` and `EntityTypeConfiguration` mapping `DepartmentName` to string column.
- Update DI in `Program.cs` to register Persistence repositories.

### EmployeeService
- Aggregate Root: `Employee`
  - Identity: `Id` (string)
  - Invariants:
    - `EmployeeName` value object (non-empty, <= 100 chars)
    - Non-negative `Age` and `Salary`
  - Relationships: Department (by `DepartmentId`), Projects (by `ProjectIds`)
  - Behaviors: `Hire`, `UpdateProfile`, `UpdateCompensation`, `AssignDepartment`, `AssignToProject`
- Value Objects:
  - `EmployeeName`

Current status:
- Added `Domain/ValueObjects/EmployeeName.cs`
- Added `Domain/Entities/Employee.cs` with behaviors above.
- Existing EF entity in `Models/Employees.cs` is still used by current DbContext; Application/Controllers remain unchanged for now.

Next steps:
- Introduce repository interface in `Domain/Repositories/IEmployeeRepository.cs` (domain model-based).
- Implement repository in `Persistence` using EF Core and map value objects.
- Adapt `EmployeeService` application service to use the Domain aggregate.

### ProjectService
- Aggregate Root: `Project`
  - Identity: `Id` (string)
  - Invariants: `ProjectName` value object (non-empty, <= 50 chars)
  - Behaviors: `Rename`
- Value Objects:
  - `ProjectName`

Current status:
- Added `Domain/ValueObjects/ProjectName.cs`.
- (Planned) Add `Domain/Entities/Project.cs` similar to Department aggregate.

Next steps:
- Add aggregate entity `Domain/Entities/Project.cs` with factory `Create(name)` and `Rename(newName)`.
- Move `IProjectRepository` to `Domain/Repositories`.
- Implement EF mapping in `Persistence` and update DI.

---

## Why these changes?
- Entities are no longer just data bags; they expose behavior and ensure valid state.
- Value Objects centralize and enforce invariants (e.g., name length), avoiding scattered validations.
- Repositories live in Domain as interfaces (domain language), while Persistence implements them (EF-specific).
- Application layer coordinates use cases and translates DTOs to Domain types.
- Controllers become thin: they validate request shape and delegate to Application.

This separation reduces coupling to EF Core and simplifies unit testing of domain logic.

---

## Migration Plan (Incremental, Safe)

1. Domain First (done for all services)
   - Create Value Objects and Aggregates with behaviors.
2. Repository Interfaces (Domain)
   - Move existing repository interfaces into `Domain/Repositories` and update namespaces to reference Domain aggregates.
3. Persistence Layer
   - Create `Persistence` folders with `DbContext`, `Configurations/*Configuration.cs` for EF Core mappings of Value Objects.
   - Implement repository classes in `Persistence/Repositories` and wire up DI in `Program.cs`.
4. Application Layer
   - Create commands/queries and services that use Domain aggregates.
   - Map incoming DTOs to Domain types and return DTOs to API.
5. API Controllers
   - Replace direct repository/service usage with Application layer calls.
6. Clean Up
   - Remove old `Models/*` EF entities once Persistence maps Domain aggregates.
   - Remove old `Interfaces/*` in favor of `Domain/Repositories`.

At the end of this migration, each microservice will fully follow DDD with clear boundaries.

---

## Notes on EF Core Value Object Mapping
- Use owned types or value conversions for Value Objects:
  - Example: map `DepartmentName` to a string column with a custom `ValueConverter<DepartmentName, string>`.
- Configure in `OnModelCreating` or via `IEntityTypeConfiguration<T>`.

---

## Next Action Items
- DepartmentServices: Move `IDepartmentRepository` to Domain and implement a `Persistence` repository that uses the new Domain aggregate + value conversion for `DepartmentName`.
- EmployeeService: Introduce domain repository interface and Persistence implementation; adapt `EmployeeService` to use `Employee.Hire(...)` etc.
- ProjectService: Add domain entity `Project` and then follow the same steps as DepartmentServices.

This document will be updated as we proceed with the steps above.
