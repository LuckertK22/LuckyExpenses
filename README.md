# LuckyExpenses Template - Professional .NET Backend Template

## Overview

Este proyecto es una plantilla base limpia, profesional y lista para producción para cualquier API backend .NET, manteniendo la arquitectura, configuración y las mejores prácticas, pero sin ninguna lógica de negocio específica.

Está diseñada para ser copiada y expandida rápidamente en nuevos proyectos .NET 10.0, siguiendo el enfoque 
[Clean Architecture] con [CQRS], [MediatR], [Entity Framework Core] y [PostgreSQL].

## Estructura

```
Solution
├── src/
│   ├── LuckyExpenses.csproj              # Proyecto único con carpetas por capas
│   ├── Program.cs                       # Bootstrap (DI, Logging, Security, CORS, Swagger)
│   └── appsettings.json                 # Configuración base
├── Application/
│   ├── Behaviors/                       # Pipeline Behaviors (Validación)
│   ├── DependencyInjection/              # Registro de servicios de aplicación
│   ├── Features/                        # Módulos de features (Authentication, Users, etc.)
│   │   ├── Authentication/              # Flujo de Auth (Login/Register)
│   │   │   ├── Common/                  # DTOs compartidos
│   │   │   ├── Login/                  # Login (Query + Handler)
│   │   │   └── Register/              # Registro (Command + Handler)
│   ├── Interfaces/                      # Interfaces de servicios
│   └── Mappings/                        # Funciones de mapeo DTO->Entity
├── Domain/                              # Entidades de negocio, contratos, excepciones
│   ├── Common/                          # BaseEntity, contratos de repositorios
│   ├── Entities/                        # Expense, Category, PaymentMethod
│   ├── Enums/                           # Enums de dominio
│   ├── Exceptions/                      # Dominio / Validación / Inf., etc.
│   └── Repositories/                    # IUnitOfWork, IExpenseRepository, etc.
├── Infrastructure/                      # Persistencia, servicios externos, autenticación
│   ├── Authentication/                  # TokenService (JWT), AuthenticationService (Register/Login)
│   ├── DependencyInjection/              # DI para AppDbContext, UnitOfWork, Auth, etc.
│   ├── Persistence/                     # AppDbContext, Configuration (Entity Framework)
│   │   ├── AppDbContext.cs
│   │   └── Configurations/              # Configuraciones por entidad (EF Core)
│   └── Repositories/                     # UnitOfWork, repos por defecto
├── WebAPI/                               # Presentación, middlewares, filtros, autenticación
│   ├── Config/                          # JWT, Opciones, etc.
│   ├── Controllers/                     # Endpoints (Health, Auth)
│   │   ├── HealthController.cs
│   │   └── AuthenticationController.cs
│   ├── Middlewares/                     # Middleware global de excepciones
│   └── Routes/                           # (Próximamente)
├── Shared/                               # Common utilities, common types
│   ├── Common/                          # Response API, utils
│   ├── Options/                          # JWTOptions
│   └── Utils/                            # StringExtensions, etc.
└── tests/ (sin included)                 # Unit & Integration (placeholder)
```

## Características clave mantenidas

### 🗂️ Arquitectura
- **Clean Architecture**: cap nezávislé vrstvy (Domain, Application, Infrastructure, WebAPI)
- **Proyectos por capas** (mantenidos como carpetas en un solo proyecto)

### 📦 Dependencias & Tecnologías
- ASP.NET Core (.NET 10.0)
- Entity Framework Core + PostgreSQL (Npgsql)
- CQRS + MediatR
- FluentValidation (pipeline de validación)
- Autenticación JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer)
- Autenticación de Usuario Integrada (Identity)
- Swagger + Scalar (API documentation)
- Middleware global de excepciones, CORS, Headers de seguridad
- Logging estructurado (serilog o logging integrado)

### 🔐 Autenticación
- ASP.NET Core Identity + JWT Bearer
- ApplicationUser personalizado (`long` Id, `FirstName`, `LastName`)
- Two-Factor simplificado (solo por correo electrónico)
- Registro → Login → `AuthenticationResponse` JWT (token + email + rol + expires)

### 🛠️ Infraestructura
- PostgreSQL (a través de conexiones de `appsettings`/`user-secrets`)
- Migraciones EF Core
- UnitOfWork para gestión de transacciones
- Dependency Injection configurado en `ServiceRegistration`

### 🧪 Testing & CI
- Lista para tests (Project.UnitTests, Project.IntegrationTests)
- Configuración `launchSettings.json` y `LuckyExpenses.http`
- Setup actualizado para pruebas futuras (compilación correcta)

## Eliminado (de la lógica de negocio de LuckyExpenses original)

*Todas las entidades concretas de negocio, casos de uso, comandos, queries, handlers, DTOs, validadores, controladores y configuraciones específicas se eliminaron.*

Lo que se **conservó** (como infraestructura):
- `Domain.Repositories` (`IBaseRepository`, `IUnitOfWork`) - reutilizable
- `Domain.Services` (`ITokenService`, `IHasherService`) - fuera de negocio específico
- `Infrastructure.Persistence.Repositories` y `Configurations`
- Configuración global de `Identity` (usuario, roles, DB)
- `AuthenticationService`, `TokenService` (genérico)
- `ValidationBehavior`, `ExceptionMiddleware`
- `AppSettings`, `Logging`, `Swagger`, `CORS`, etc.

## Cómo comenzar

### Requisitos previos
- .NET 10.0 SDK
- Contenedor PostgreSQL (ejecutando en `localhost:5432`)

### Construir y ejecutar

```bash
# 1. Restaurar dependencias y build
$ dotnet restore
$ dotnet build

# 2. Ejecutar la API (por defecto en http://localhost:5000)
$ dotnet run --launch-profile https

# O usa `dotnet run --launch-profile https` si quieres HSTS en producción
```

### Configuración

Los archivos de configuración se basan en claves:

**`appsettings.json` (base, incluir en git)**
```json
{
  "Logging": {
    "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=lucky_expenses;Username=postgres"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA_DESDE_USER_SECRETS_O_ENTORNO",
    "Issuer": "https://localhost:3000",
    "Audience": "lucky-expenses"
  }
}
```

**Secrets de desarrollo (`user-secrets` – omitir del git)**

```bash
# Inicia la plantilla con secrets (incluye la contraseña)
$ dotnet user-secrets init
$ dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=lucky_expenses;Username=postgres;Password=postgres"
$ dotnet user-secrets set "Jwt:Key" "TU_CLAVE_SECRETA_AQUI"

# La plantilla ahora leerá ambos secretos en desarrollo
dotnet run --launch-profile https
```

### Endpoints disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/Health` | Comprueba el estado (sin autenticación) |
| `POST` | `/api/authentication/register` | Registra un nuevo usuario (público) |
| `POST` | `/api/authentication/login` | Login y devuelve el JWT (público) |

### Documentación

- **Swagger UI**: `http://localhost:5000/swagger`
- **Scalar API Reference**: `http://localhost:5000/scalar`

Agrega autenticación (Bearer) en Swagger para probar endpoints protegidos en el futuro.

## Ejemplo de request/response

**Registrar**
```bash
curl -X POST http://localhost:5000/api/authentication/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "password": "MySecretPassword123"
  }'
```

**Login**
```bash
curl -X POST http://localhost:5000/api/authentication/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "MySecretPassword123"
  }'
```

**Respuesta** (200 OK)
```json
{
  "token": "eyJhbGciOiJIUzI...",
  "email": "john@example.com",
  "role": "USER",
  "expiresAt": "2026-08-02T20:45:00.000Z"
}
```

## Extender la plantilla

### Añadir un nuevo módulo (ejemplo: Users)

1. **Dominio**: Agregar nuevas entidades (`UserProfile`, etc.) en `Domain/Entities`
2. **Configuración**: Nueva `UserProfileConfiguration` en `Infrastructure/Persistence/Configurations`
3. **Repositorio**: Nuevo `IUserProfileRepository` en `Domain/Repositories`
4. **Caso de uso**: Nuevo Feature (`Users`) en `Application/Features/` (Queries/Commands)
5. **Servicio**: Nuevo `IUserProfileService` en `Infrastructure/` y su implementación
6. **API**: Nuevo `UserController` en `WebAPI/Controllers`

### Proyectos de testing

La plantilla incluye `tests/` como marcadores de posición. Para ellos:

```bash
mkdir tests/Project.UnitTests
mkdir tests/Project.IntegrationTests
```

Puedes actualizar su `*.csproj` con referencias a los proyectos `src/`.

## Prácticas recomendadas y convenciones

- **Clean Architecture**: las dependencias fluyen de adentro hacia afuera (Dominio → Aplicación → Infraestructura → API)
- **Tipos**: `long` para claves primarias, `string` para emails, `DateTime.UtcNow` para marcas de tiempo.
- **Seguridad**: las claves JWT y de base de datos nunca deben incluirse en el control de código fuente.
- **Documentación**: cada feature es un directorio independiente (ej., `Application/Features/Authentication/`) con Requests, Commands/Queries, Handlers y Validadores para un código limpio y centralizado.
- **Testing**: El pipeline de validación (`ValidationBehavior`) funciona automáticamente en todos los comandos/queries.

## Base para futuros módulos (plantilla)

Los siguientes módulos son como **ejemplos de estructuras**, pero sin lógica implementada (solo firmas de archivos para mostrar cómo se desglosarían):

```
Application/Features/Users/
  ├── Common/UsersDto.cs
  ├── Commands/CreateUserCommand.cs
  ├── Queries/GetUserQuery.cs
  └── Services/IUsersService.cs

Application/Features/Expenses/
  ├── Common/ExpenseDto.cs
  ├── Commands/CreateExpenseCommand.cs
  └── Services/IExpenseService.cs

Application/Features/Categories/
  └── Commands/CreateCategoryCommand.cs

WebAPI/Controllers/UsersController.cs
WebAPI/Controllers/ExpensesController.cs
```

## Licencia

Template generado para uso interno. Se invita a modificar, ajustar y reutilizar bajo la licencia propia del propietario del proyecto.

## Contacto

Para preguntas, problemas o extensiones, contacta a los propietarios del repositorio.
