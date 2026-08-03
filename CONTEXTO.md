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
