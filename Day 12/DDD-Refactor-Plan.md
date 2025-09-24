# Domain-Driven Design (DDD) Refactor Plan for Microservices

This document is a practical, incremental checklist to evolve your existing microservices to a DDD-aligned structure without breaking behavior.

Applies to services under `Services/`:
- `EmployeeService/`
- `DepartmentServices/`
- `ProjectService/`

---

## 0) Guiding Principles
- Keep controllers thin; no domain logic in API layer.
- Domain is persistence-ignorant; EF Core lives in Infrastructure.
- Encapsulate invariants and behavior inside aggregate roots.
- Application layer orchestrates use cases and calls external systems via ports (interfaces).

---

## 1) Introduce Layered Structure (Non-breaking)
For each service, create folders (single project approach):
- `Api/`
- `Application/`
  - `DTO/`, `Ports/`, `Services/` (or later `Commands/`, `Queries/`)
- `Domain/`
  - `Entities/`, `ValueObjects/`, `Repositories/`, `Services/` (domain services if needed)
- `Infrastructure/`
  - `Persistence/` (DbContext, EF configs)
  - `Repositories/` (EF implementations)
  - `Http/` (HTTP client adapters)

Do not move files yet; just scaffold folders.

---

## 2) Define Domain Model (Per Service)
Start with `EmployeeService` as the pattern.

- Entities/Aggregates (`Domain/Entities/`):
  - Employee (Aggregate Root)
  - Manager (optional subtype/role)
- Value Objects (`Domain/ValueObjects/`):
  - EmployeeId, DepartmentId, ProjectId
  - Money (for Salary)
  - PersonName (optional)
- Repository interfaces (`Domain/Repositories/`):
  - `IEmployeeRepository`
- Behaviors and invariants in `Employee`:
  - Validate Name not empty
  - Validate Age >= 18
  - Validate Salary >= 0
  - Methods: `AssignDepartment(DepartmentId)`, `AddProject(ProjectId)`, `RemoveProject(ProjectId)`, `UpdateCompensation(Money)`

Mirror similar modeling for:
- DepartmentService: `Department` (Aggregate Root), `DepartmentId`, `DepartmentName` (VO), `IDepartmentRepository`.
- ProjectService: `Project` (Aggregate Root), `ProjectId`, `ProjectName` (VO), `IProjectRepository`.

---

## 3) Move Existing Code to Matching Layers
- Move EF Core classes to `Infrastructure/Persistence/`:
  - `Data/dbContext.cs` -> `Infrastructure/Persistence/<Service>DbContext.cs`
- Move repository implementations to `Infrastructure/Repositories/`:
  - `Repository/EmployeeRepository.cs` -> `Infrastructure/Repositories/EmployeeRepository.cs`
- Move repository interfaces into `Domain/Repositories/` (contracts only).
- Move HTTP clients to `Infrastructure/Http/`.
- Keep controllers in `Api/Controllers/`.

Update namespaces accordingly after each move.

---

## 4) Application Layer Orchestration
- Move current service classes to `Application/Services/` (e.g., `EmployeeService.cs` -> `Application/Services/EmployeeAppService.cs`).
- Define ports (interfaces) for cross-service calls in `Application/Ports/`:
  - `IDepartmentGateway`, `IProjectGateway` (replace framework-specific HTTP interfaces).
- Keep DTOs in `Application/DTO/`.
- Ensure Application services depend on:
  - Domain repository interfaces (e.g., `IEmployeeRepository`).
  - Application ports (`IDepartmentGateway`, `IProjectGateway`).
  - Domain entities/VOs for business rules.

Later, when doing CQRS, split into Commands/Queries with MediatR.

---

## 5) Infrastructure Adapters
- Implement Application ports in `Infrastructure/Http/`:
  - `DepartmentGateway` (wraps actual HTTP client), `ProjectGateway`.
- Implement domain repository interfaces with EF Core in `Infrastructure/Repositories/`.
- Keep DbContext and EF configurations in `Infrastructure/Persistence/` only.

---

## 6) API Layer
- Controllers remain in `Api/Controllers/` and should only call Application services.
- Remove any domain logic from controllers.

---

## 7) Dependency Injection Wiring
In each service `Program.cs`:
- Register DbContext
- Register repositories: `IEmployeeRepository -> EmployeeRepository`
- Register gateways: `IDepartmentGateway -> DepartmentGateway`, `IProjectGateway -> ProjectGateway`
- Register application services: `IEmployee -> EmployeeAppService` (or rename interface appropriately)
- Add HttpClient registrations for gateways if needed.

---

## 8) Incremental Refactor Order (Suggested)
Perform these steps per service, starting with EmployeeService:

1. Scaffold folders (Section 1).
2. Create domain entities + value objects with invariants (Section 2).
3. Move repository interfaces to Domain; adjust namespaces.
4. Move EF DbContext + repos to Infrastructure; adjust namespaces.
5. Introduce Application ports; adapt existing service class to use ports.
6. Move service class to Application; rename to `*AppService`.
7. Create Infrastructure HTTP adapters implementing ports.
8. Update DI in `Program.cs`.
9. Run and test endpoints; fix namespaces and wiring.
10. Repeat for DepartmentService and ProjectService.

---

## 9) Testing
- Unit tests for Domain:
  - Employee invariants (age, salary, name)
  - Methods (assign department, add/remove project)
- Application tests:
  - Orchestration logic using mocks for repositories and gateways.

---

## 10) Optional: Multi-project Clean Architecture
If you want stronger boundaries later, split each service into projects:
- `<Service>.Domain` (class library)
- `<Service>.Application` (class library)
- `<Service>.Infrastructure` (class library)
- `<Service>.Api` (ASP.NET Core Web API)

Adjust project references accordingly:
- Api -> Application
- Application -> Domain
- Infrastructure -> Domain, Application (if implementing ports)
- Api -> Infrastructure

---

## Quick Checklist (Per Service)
- [ ] Folders: Api, Application, Domain, Infrastructure
- [ ] Domain entities and value objects created
- [ ] Domain repository interfaces defined
- [ ] EF Core moved to Infrastructure/Persistence
- [ ] Repositories moved to Infrastructure/Repositories
- [ ] HTTP gateways implemented in Infrastructure/Http
- [ ] Application services using ports + domain
- [ ] Controllers only call Application services
- [ ] DI wired in Program.cs
- [ ] Unit tests for domain and application

---

## Notes Tailored to Current Code
- Current `EmployeeService` mixes domain logic and orchestration (e.g., department/project creation). Move these decisions into Application, but keep pure rules (like assignment methods and validation) in the Employee aggregate.
- Replace `IDepartmentHttpClient` and `IProjectHttpClient` with `IDepartmentGateway` and `IProjectGateway` in the Application layer. Implement them under Infrastructure/Http by delegating to your existing clients.
- Keep existing DTOs under Application/DTO for now; you’ll later adapt them to commands/queries when you do CQRS with MediatR.

---

## Done Criteria for the DDD Assignment
- Entities, Aggregates, and Value Objects defined per service.
- Domain logic encapsulated in aggregate roots.
- Clear layer boundaries: Domain, Application, Infrastructure, Api.
- Controllers thin, Application services orchestrate, Infrastructure adapts.
- Tests in place for domain invariants and application use cases.
