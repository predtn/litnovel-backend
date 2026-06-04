# LitNovel — Architecture

## 1. Project Overview

LitNovel is a web novel reading platform built with Clean Architecture on ASP.NET Core.

## 2. Tech Stack

| Concern | Technology |
|---|---|
| Runtime | .NET 10 |
| Web framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Password hashing | BCrypt.Net-Next |
| Validation | FluentValidation |
| API docs | Swagger (Swashbuckle) |
| HTTP client | `HttpClient` (typed, registered via `AddHttpClient<T>`) |

---

## 3. Dependency Graph

```
LitNovel.Domain
       ↑
LitNovel.Application
       ↑                    ↑
LitNovel.Infrastructure     |
       ↑                    |
LitNovel.WebAPI ────────────┘ (references Infrastructure for DI wiring only)
```

Actual project references:
```
Domain          → none
Application     → Domain
Infrastructure  → Domain, Application
WebAPI          → Application, Infrastructure
```

**Key rules:**
- `Domain` has zero external dependencies — pure C# only.
- `Application` knows abstractions and domain models only. No EF Core, no HTTP.
- `Infrastructure` implements interfaces defined in `Application`.
- `WebAPI` references `Infrastructure` solely to call `AddInfrastructure()` in `Program.cs`.
- Controllers never inject `DbContext`, repository implementations, or Infrastructure services directly.
- All dependencies flow inward toward the Domain layer.

---

## 4. Request Flow

```
Client (Browser)
  │
  ▼
[LitNovel.MVC] Controller
  │ HttpClient → GET /api/novels
  ▼
[LitNovel.WebAPI] Controller
  │ calls IUseCase interface
  ▼
[LitNovel.Application] UseCase
  │ validates + business logic
  ▼
[LitNovel.Infrastructure]
  │
  ├── Repository → LitNovelContext → SQL Server
  └── Services (JWT, Password, External APIs)
  │
  ▼
Client
```
---

## 5. Runtime Composition

DI wiring happens at startup in `Program.cs`:

```
AddApplication()
  ILoginUseCase           → LoginUseCase
  IGetNovelUseCase        → GetNovelUseCase
  IGetChapterUseCase      → GetChapterUseCase
  ...

AddInfrastructure(config)
  LitNovelContext         → SQL Server
  IUserRepository         → UserRepository
  INovelRepository        → NovelRepository
  IUnitOfWork             → UnitOfWork
  IJwtService             → JwtService
  IPasswordService        → PasswordService
```

DI conventions:
- UseCases, repositories, and services use `AddScoped<>`.
- Stateless utilities may use `AddSingleton<>`.
- `ExceptionHandlingMiddleware` is registered as scoped because it implements `IMiddleware`.
- Infrastructure types are registered only inside Infrastructure.

---


## 6. Database

**Migration commands:**
```bash
dotnet ef migrations add <Name> --project LitNovel.Infrastructure --startup-project LitNovel.WebAPI --context LitNovelContext --output-dir Persistences/Migrations

dotnet ef database update --project LitNovel.Infrastructure --startup-project LitNovel.WebAPI --context LitNovelContext
```

Database conventions:
- Lazy loading is forbidden.
- Read-only queries use `AsNoTracking()`.
- Large include graphs use `AsSplitQuery()`.
- Queries should project only required fields using `Select()`.
- Enums are stored as strings.
- All persistence operations are async.

Transaction conventions:
- Multi-entity write operations use `IUnitOfWork`.
- `SaveChangesAsync()` is called once per UseCase.
- Transaction orchestration belongs in UseCases, never Controllers.

Audit behavior in `LitNovelContext.SaveChangesAsync()`:
- Automatically updates `UpdatedAt`.
- Preserves `CreatedAt`.

---