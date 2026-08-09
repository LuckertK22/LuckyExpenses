# LuckyExpenses

API de gastos personales construida con **ASP.NET Core**, **Clean Architecture**, **CQRS + MediatR**, **Entity Framework Core** y **PostgreSQL**, con un frontend en **Angular**.



## Stack backend

- ASP.NET Core (.NET 10)
- Clean Architecture (Domain / Application / Infrastructure / WebAPI / Shared)
- CQRS + MediatR
- Entity Framework Core + Npgsql (PostgreSQL)
- FluentValidation
- JWT (HS256) + Refresh Tokens con rotación y detección de reuso
- Swagger UI + Scalar (documentación OpenAPI)
- Security headers + CORS + ExceptionHandler middleware

## Arquitectura

La solución se abre desde `backend/LuckyExpenses.slnx` y se organiza en capas:

```
backend/
├── Domain/          # Entidades, enums, excepciones, interfaces de repositorios
├── Application/     # Casos de uso (Features: Command/Query + Handler + Validator + Response)
├── Infrastructure/  # EF Core (DbContext), repositorios, autenticación, servicios
├── WebAPI/          # Controllers, filtros, middleware, configuración de DI y JWT
├── Shared/          # Opciones de configuración (JwtOptions)
└── Program.cs       # Pipeline, Swagger/Scalar, migraciones al arranque
```

Principios aplicados:

- Los controllers son delgados: inyectan `ISender` (MediatR) y delegan en handlers.
- El usuario autenticado se resuelve con `ICurrentUser` (desde el token JWT), nunca desde el body.
- Las respuestas se envuelven en `ApiResponse<T>` (`{ ok, message, data }`) mediante `GlobalResponseFilter`.
- CQRS separa comandos (escritura) de queries (lectura); cada feature vive en `Application/Features/{Modulo}/Command|Query/{Feature}/`.

## Funcionalidades

- **Autenticación**: register, login, refresh (rotación de tokens), logout. Passwords hasheados, refresh tokens hasheados con SHA-256 y de un solo uso.
- **Expenses**: CRUD completo con filtros (fechas, categoría, método de pago, búsqueda de texto con ILike + Unaccent) y paginación.
- **Catálogos globales**: categorías y métodos de pago (compartidos por todos los usuarios).
- **Dashboard**: resumen del mes (totales, promedio, variación vs mes anterior, desglose por categoría).

## Endpoints principales

Todos bajo el prefijo `/api/v1`:

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/v1/Authentication/Register` | Registro de usuario |
| POST | `/api/v1/Authentication/Login` | Login (devuelve access + refresh token) |
| POST | `/api/v1/Authentication/Refresh` | Rotación de tokens |
| POST | `/api/v1/Authentication/Logout` | Revoca el refresh token |
| GET/POST/PUT/DELETE | `/api/v1/Expenses` | CRUD de gastos |
| GET | `/api/v1/Categories/GetCategories` | Catálogo paginado de categorías |
| GET | `/api/v1/PaymentMethods/GetPaymentMethods` | Catálogo paginado de métodos de pago |
| GET | `/api/v1/Dashboard/Summary` | Resumen del mes |
| GET | `/api/v1/Health/Health` | Health check (sin autenticación) |

## Requisitos

- .NET SDK 10
- PostgreSQL (local o Docker)
- Docker (opcional, para el stack completo)

## Ejecutar en local

1. Crear la base de datos y configurar la cadena de conexión (user secrets o `appsettings.json`):
   `ConnectionStrings:DefaultConnection` con formato Npgsql `Host=...;Port=5432;Database=lucky_expenses;Username=postgres`.
2. Configurar `Jwt:Key` (mínimo 32 caracteres, clave HS256).
3. Ejecutar:

```bash
cd backend
dotnet run
```

En Development se habilitan Swagger UI y Scalar:

- Scalar: http://localhost:5003/scalar/v1
- Swagger UI: http://localhost:5003/swagger/index.html

Las migraciones de EF Core se aplican automáticamente al arrancar (`Database:MigrateOnStartup`, default `true`).

## Ejecutar con Docker Compose

El `docker-compose.yml` de la raíz levanta PostgreSQL, API y frontend:

```bash
docker compose up -d --build
```

- API: http://localhost:5003
- Frontend: http://localhost:4200
- PostgreSQL: `localhost:5433`

Las variables secretas locales se cargan desde `.env` (gitignored): `POSTGRES_PASSWORD` y `JWT_KEY`.

## Configuración (variables de entorno)

En .NET las variables usan doble guion bajo por sección (`Seccion__Clave`):

| Variable | Requerida | Descripción |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | Sí | Cadena Npgsql hacia PostgreSQL |
| `Jwt__Key` | Sí | Clave de firma HS256 (secreta, mínimo 256 bits) |
| `ASPNETCORE_ENVIRONMENT` | No | `Development` / `Production` |
| `ASPNETCORE_URLS` | No | Puerto de escucha de Kestrel |
| `Database__MigrateOnStartup` | No | Aplica migraciones al arrancar (default `true`) |
| `Documentation__Enabled` | No | Habilita Swagger/Scalar en cualquier ambiente (default `true`) |
| `Jwt__Issuer` / `Jwt__Audience` | No | Emisor/audiencia del token (defaults en `appsettings.json`) |
| `Jwt__AccessTokenExpirationMinutes` / `Jwt__RefreshTokenExpirationDays` | No | Vigencias de tokens (defaults 15 min / 7 días) |

## Producción (Render)

- **Backend**: servicio Web de Render construido desde `backend/Dockerfile` (multi-stage `sdk:10.0-noble` → `aspnet:10.0-noble`, non-root, health check sobre `/api/v1/Health/Health`). Health check de Render: `/api/v1/Health/Health`.
- **Base de datos**: PostgreSQL managed de Render. Usar la cadena Npgsql en formato key=value (no `postgres://`).
- **Frontend**: `frontend/Dockerfile` con nginx; proxya `/api/v1` hacia el backend (`BACKEND_URL`) con SNI habilitado y `Host $proxy_host` (evita el loop de enrutamiento de Render). `listen ${PORT}` para el puerto que inyecta Render.

Ver [`CONTEXTO.md`](CONTEXTO.md) para el detalle completo del despliegue y las variables de cada servicio.

## Seguridad

- Headers en todas las respuestas: `X-Content-Type-Options: nosniff`, `X-Frame-Options: DENY`, `X-XSS-Protection`, `Referrer-Policy: no-referrer`.
- Passwords y refresh tokens hasheados (nunca en texto plano).
- Detección de reuso de refresh tokens: revoca toda la familia activa del usuario.
- CORS permisiva (`AllowAnyOrigin`) pensada para el frontend desplegado en otro origen.

## Migraciones

Las migraciones viven en `backend/Migrations/` y se aplican automáticamente al arrancar. Para generarlas:

```bash
cd backend
dotnet ef migrations add NombreDeLaMigracion
```

## Documentación adicional

- [`CONTEXTO.md`](CONTEXTO.md): contexto completo del proyecto (arquitectura, decisiones, infraestructura y producción).
- [`frontend/README.md`](frontend/README.md): guía del frontend Angular.
