# LuckyExpenses - Contexto del proyecto

## Objetivo

Construir una API backend profesional usando ASP.NET Core, Clean Architecture, CQRS, EF Core y PostgreSQL como proyecto de portafolio.

## Stack

- ASP.NET Core (.NET)
- Clean Architecture
- CQRS + MediatR
- Entity Framework Core
- PostgreSQL
- Docker + WSL
- Swagger
- FluentValidation (más adelante)
- JWT (Refresh Tokens se dejan para una segunda versión)

## Infraestructura

- Docker funcionando en WSL.
- Contenedor PostgreSQL reutilizado.
- Contenedor pgAdmin reutilizado.
- Base de datos creada: `lucky_expenses`.

## Arquitectura

`Domain` `Application` `Infrastructure` `Api` `Shared`

## Convenciones de código (estilo FTF)

Se copiaron la estructura de carpetas de `Features` y el estilo de controllers del proyecto de referencia FTF (`FtfApiClient`). El proyecto FTF NO se modifica.

- Features en `Application/Features/{Modulo}/Command|Query/{Feature}/` con un archivo por rol:
  - `{Feature}Command.cs` / `{Feature}Query.cs`
  - `{Feature}CommandHandler.cs` / `{Feature}QueryHandler.cs`
  - `{Feature}CommandValidator.cs` (FluentValidation)
  - `{Feature}Response.cs` (solo si la feature devuelve datos útiles)
- **El Command ES el payload del POST**: clase con `{ get; set; }`, se envía directo con `[FromBody]`. No hay `Request` separado.
- Las respuestas (`*Response`) son **clases** con `{ get; set; }`, no records (como en FTF).
- Los controllers son delgados: `[Route("api/v1/[controller]")]`, inyectan `ISender`, y hacen `_sender.Send(command)` sin lógica extra.
- Se mantiene `GlobalResponseFilter`: los controllers devuelven datos crudos y el filtro envuelve en `ApiResponse<T>` (`{ ok, message, data }`).
- Los handlers resuelven el usuario autenticado con `ICurrentUser` (nunca desde el body ni parseando claims en el controller).

## Usuario autenticado (ICurrentUser)

- `Application/Context/ICurrentUser.cs` + `Infrastructure/Context/CurrentUser.cs` (registrado como Scoped).
- Expone `IsAuthenticated`, `UserId`, `Email`, `Role` y `GetUserAsync()`, leídos del token JWT vía `IHttpContextAccessor` (con cache por request).
- Patrón copiado de FTF. Por eso `CreateExpenseCommand` NO lleva `UserId`: lo resuelve el handler desde `_currentUser.UserId`.

## Estructura actual de Features

```
Application/Features/
├── Authentication/
│   ├── Command/Login/       LoginCommand, LoginCommandHandler, LoginCommandValidator, LoginResponse
│   └── Command/Register/    RegisterCommand, RegisterCommandHandler, RegisterCommandValidator
└── Expenses/
    ├── Command/CreateExpense/  CreateExpenseCommand, CreateExpenseCommandHandler, CreateExpenseCommandValidator, CreateExpenseResponse
    ├── Command/UpdateExpense/  UpdateExpenseCommand, UpdateExpenseCommandHandler, UpdateExpenseCommandValidator, UpdateExpenseResponse
    ├── Command/DeleteExpense/  DeleteExpenseCommand, DeleteExpenseCommandHandler, DeleteExpenseCommandValidator
    ├── Query/GetExpenses/      GetExpensesQuery, GetExpensesQueryHandler, GetExpensesQueryValidator, GetExpensesResponse
    └── Query/GetExpenseById/   GetExpenseByIdQuery, GetExpenseByIdQueryHandler, GetExpenseByIdQueryValidator, GetExpenseByIdResponse

Application/Mappers/         ExpenseMapper.cs (mappers compartidos por todas las features)
```

- `Login` devuelve `LoginResponse` (Token, Email, Role, ExpiresAt).
- `Register` no devuelve nada (solo confirma).
- `CreateExpense` devuelve `CreateExpenseResponse` (Id, CategoryId, PaymentMethodId, Title, Description, Amount, ExpenseDate, CreatedAt).
- `UpdateExpense` devuelve `UpdateExpenseResponse` (mismos campos que `CreateExpenseResponse`). Valida referencias y dueño del gasto (`GetByIdForUserAsync`).
- `DeleteExpense` no devuelve nada (solo confirma). Verifica dueño antes de eliminar (`Remove` + `SaveChangeAsync`).
- `GetExpenses` devuelve `PagedResponse<GetExpensesResponse>` (lista paginada con Id, CategoryId, CategoryName, PaymentMethodId, PaymentMethodName, Title, Description, Amount, ExpenseDate, CreatedAt, más TotalItems/Page/Size).
- `GetExpenseById` devuelve `GetExpenseByIdResponse` (mismos campos que un item de la lista). No expone gastos de otros usuarios (filtra por `UserId`).
- `GetExpenses` devuelve `PagedResponse<GetExpensesResponse>` (lista paginada con Id, CategoryId, CategoryName, PaymentMethodId, PaymentMethodName, Title, Description, Amount, ExpenseDate, CreatedAt, más TotalItems/Page/Size).
- `GetExpensesQuery` filtra por FromDate, ToDate, CategoryId, PaymentMethodId y Title (búsqueda de texto con ILike + Unaccent), con Page/Size (defaults 1/10). El `UserId` lo resuelve el handler con `ICurrentUser`.
- La búsqueda de texto necesita la extensión `unaccent` de Postgres, habilitada por la migración `EnableUnaccent` (`CREATE EXTENSION IF NOT EXISTS unaccent`).
- Para exponer los nombres de catálogos, `Expense` tiene navegaciones `Category` y `PaymentMethod` (configuradas con `HasOne(e => e.Category/…)`).
- `IExpenseRepository` extiende `IBaseRepository<Expense, long>` y agrega `GetByUserAsync` (filtros + `Include` de categorías, devuelve tupla `(TotalCount, Items)`) y `GetByIdForUserAsync` (gasto por id del usuario actual). Los filtros se pasan como parámetros (el `GetExpensesQuery` ya es el portador de filtros; no hay clase `ExpenseFilter`).

## Paginación genérica

- `IBaseRepository<TEntity, TKey>` expone `GetPagedAsync(IQueryable<TEntity>, page, size)` que hace `CountAsync()` antes de paginar y devuelve `(TotalCount, Items)`.
- `Infrastructure/Repositories/BaseRepository.cs` lo implementa sobre `context.Set<TEntity>()`; los repositorios concretos lo heredan.
- El repositorio concreto construye su `IQueryable` (filtros condicionales + `Include` + orden) y delega el paginado a `GetPagedAsync`.
- El handler recibe `(TotalCount, Items)` y lo mapea a `PagedResponse<T>` (`Items`, `TotalItems`, `Page`, `Size`) para que el frontend conozca total y página actual.

## AuthenticationService

`Infrastructure/Authentication/AuthenticationService.cs` implementa `IAuthenticationService`:
- `RegisterAsync(RegisterCommand)`: valida email único → crea `User` con password hasheado (rol USER, estado ACTIVE) → guarda. `ConflictException` si el email ya existe.
- `LoginAsync(LoginCommand)`: busca por email → verifica hash (`IHasherService`) → valida estado ACTIVE → genera JWT (`ITokenService`, expira en 8h) → devuelve `LoginResponse`.
- Dependencias: `IUserRepository`, `IHasherService`, `ITokenService`, `IUnitOfWork`.

## Orden de desarrollo acordado

1. Modelar el dominio.
2. Crear Application (casos de uso).
3. Implementar Infrastructure.
4. Exponer la API.

## Dominio inicial

- `Common/` - `BaseEntity` - `AuditableEntity`
- `Entities/` - `User` - `Expense` - `Category` - `PaymentMethod`
- `Enums/` - `UserRoleEnum` - `UserStateEnum`
- `Exceptions/` - `DomainException` - `NotFoundException` - `InvalidCredentialsException` - `CustomValidationException`
- `Repositorios/` - `IUserRepository` - `IExpenseRepository` - `ICategoryRepository` - `IPaymentMethodRepository` - `IUnitOfWork`

## Decisiones de modelado

- Usar `Id` como clave primaria en todas las entidades.
- Usar `UserId`, `CategoryId` y `PaymentMethodId` para relaciones.
- Renombrar `Password` a `PasswordHash`.
- `Role` y `State` serán enums.
- `CreatedAt` y `UpdatedAt` vivirán en `AuditableEntity`.
- `Description` permanecerá como propiedad de `Expense`, no como entidad.
- `Category` y `PaymentMethod` serán entidades para permitir crecimiento futuro.
- `Category` y `PaymentMethod` se manejan como **catálogos globales** (sin `UserId`), compartidos por todos los usuarios y sembrados con datos base vía migración (`MakeCatalogsGlobal`).
- Los catálogos tienen `Code` (identificador estable, único) y `Name` único (índices únicos en `categories` y `payment_methods`).
- `expenses` referencia a los catálogos por id: `category_id` (obligatorio) y `payment_method_id` (opcional).
- FKs de `expenses` (migración `AddExpenseForeignKeys`): `user_id` → `users` (RESTRICT), `category_id` → `categories` (RESTRICT), `payment_method_id` → `payment_methods` (SET NULL).

## Modelo inicial de Expense

- `Id`
- `UserId`
- `CategoryId`
- `PaymentMethodId`
- `Title`
- `Description`
- `Amount`
- `ExpenseDate`

## Modelo inicial de User

- `Id`
- `FirstName`
- `LastName`
- `Email`
- `PasswordHash`
- `Role`
- `State`

## Módulos previstos

- Authentication
- Users
- Expenses
- Categories
- PaymentMethods
- Dashboard
- Reports (V2)
- Budgets (V2)
- Notifications (V2)

## Notas

- No implementar Refresh Tokens por ahora.
- Construir el proyecto como si fuera un sistema listo para producción.
- Evitar sobreingeniería en la primera versión.
- Rutas bajo prefijo `api/v1` (ej. `/api/v1/expenses/Create`).
- El endpoint `GetCurrentUser` se eliminó por decisión del usuario (también `IAuthenticationService.GetUserAsync` y la feature `Query/GetCurrentUser`).
- Los catálogos se traen con `Include` en consultas de gastos para exponer sus nombres sin consultas extra.
