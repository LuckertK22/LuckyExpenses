# Plan del Frontend — LuckyExpenses

> Documento vivo del plan de implementación del frontend Angular. Se actualiza al final de cada fase. Ver `CONTEXTO.md` para el contrato del backend (fuente de verdad).

## Estado actual

- **Fase 1 — código completo + build validado.**
- **Fase 2 — código completo + tests/build validados.**
- **Fase 3 — código completo + tests/build validados + probado por el usuario.**
- **Fase 4 — código completo + tests/build validados + probado por el usuario.**
- **Fase 5 — código completo + tests/build validados + probado por el usuario.**
- **Fase 6 — código completo + tests/build validados.**
- Pendiente: **Fase 7**.

## Decisiones de diseño (confirmadas por el usuario)

| Tema | Decisión |
|---|---|
| Estilo visual | Financiero/SaaS limpio |
| Tema | Light + Dark con toggle (persistido en `localStorage` bajo `le.theme`, default `prefers-color-scheme`) |
| Paleta | Verde esmeralda (acento primario) + neutros |
| Layout autenticado | Sidebar fijo + topbar; en móvil el sidebar es overlay drawer (hamburguesa) |
| Responsive | Mobile-first |
| Dashboard | KPIs + comparativa barras (mes actual vs anterior) + dona por categoría. **Sin serie de 12 meses** (el backend solo devuelve mes actual vs anterior; no tocar backend) |
| Gráficos | `ng-apexcharts` (+ peer `apexcharts`) — **única dependencia nueva** |
| Expenses: listado | Tabla en desktop / cards en móvil |
| Expenses: crear/editar | Modal reutilizable (mismo componente para Create y Edit) |
| Expenses: eliminar | Modal de confirmación |
| Expenses: filtros | Barra de filtros arriba de la tabla (fromDate, toDate, categoryId, paymentMethodId, title) + Limpiar |
| Librería UI | **Ninguna** — componentes propios con SCSS + design tokens |
| Layout Auth | Tarjeta centrada con branding |
| Tokens | Access token **en memoria** (signal, se rehidrata con refresh al arranque/401); refresh token en `localStorage` bajo `le.refresh` |
| Caché catálogos | Categories y PaymentMethods se cachean en signals (datos globales y estables) |

## Decisiones técnicas clave

1. **HttpClient** con `provideHttpClient(withInterceptors([authInterceptor, errorInterceptor]))`. Los componentes NUNCA construyen headers de auth.
2. **authInterceptor**: adjunta `Bearer` (excepto a endpoints `/Authentication/*`); en 401 hace refresh con cola (evita refresh simultáneos); si el refresh falla → logout + redirect a `/authentication/login`.
3. **errorInterceptor**: parsea `ProblemDetails` (RFC 7807) y muestra toast; en 400 de validación extrae el primer mensaje de `errors`.
4. **Estado**: signals + RxJS. **Sin NgRx** ni state management complejo. `AuthService.user`/`isAuthenticated` como signals.
5. **Formularios**: Reactive Typed Forms (`FormBuilder.nonNullable`). Validación cliente refleja FluentValidation pero el servidor sigue siendo la autoridad; los errores 400 del backend se mapean a campos.
6. **Routing**: `canMatch(authGuard)` en protegidas, `canActivateMatch(guestGuard)` en auth; lazy `loadComponent` por feature.
7. **Design tokens**: variables CSS en `styles.scss` (`--le-*`), tema con atributo `[data-theme]` en `<html>` vía `ThemeService`.
8. **Paginación**: componente `pagination` reutilizable con signals.
9. **`resource()` de Angular 22**: el loader solo se re-ejecuta si se declara la opción `params` (señal reactiva) o se llama `reload()`. Las señales leídas dentro del `loader` NO disparan recarga por sí solas (lección aplicada en `expenses-list`: paginación y filtros pasan por `params`).

## Estructura de carpetas

```text
frontend/src/app/
├── core/
│   ├── guards/          auth.guard.ts (authGuard, guestGuard)
│   ├── interceptors/    auth.interceptor.ts, error.interceptor.ts
│   ├── services/        token.service.ts, auth.service.ts, theme.service.ts, toast.service.ts, dashboard.service.ts, expense.service.ts, reference.service.ts
│   └── models/          api-response, paged-response, problem-details, auth, expense, reference, dashboard
├── shared/
│   ├── components/      not-found, button, input, select, spinner, modal, toast, confirm-dialog, pagination, empty-state
│   └── utils/           (pendiente)
├── layout/
│   ├── auth-layout/     auth-layout.ts
│   └── authenticated-layout/  authenticated-layout.ts
├── features/
│   ├── authentication/  login/, register/
│   ├── dashboard/       dashboard/
│   ├── expenses/        expenses-list/, expense-form-modal/
│   ├── categories/      categories-list/
│   └── payment-methods/ payment-methods-list/
├── app.config.ts        (HttpClient + interceptors + router + zoneless)
├── app.routes.ts        (lazy + guards)
├── app.ts / app.html / app.scss
src/environments/        environment.ts (prod: /api/v1), environment.development.ts (dev: http://localhost:5003/api/v1)
src/styles.scss          (design tokens light/dark + reset base)
src/index.html           (lang es, fuente Inter, título LuckyExpenses)
```

## Contrato del backend (resumen; ver `CONTEXTO.md` para detalle)

- Envoltorio de éxito: `ApiResponse<T>` = `{ ok, message, data }` (camelCase en JSON).
- Errores: `ProblemDetails` = `{ type, title, status, detail, instance, errors? }` (content-type `application/problem+json`).
- Paginación: `PagedResponse<T>` = `{ items, totalItems, page, size }`.
- Endpoints (base `api/v1`, prefijo de controller con mayúscula):
  - `POST /Authentication/Login|Register|Refresh|Logout` (públicos)
  - `POST /Expenses/Create`, `GET /Expenses/GetExpenses`, `GET /Expenses/GetExpenseById?id=`, `PUT /Expenses/Update`, `DELETE /Expenses/Delete` (JWT; **Delete y Update llevan el id en el body**)
  - `GET /Categories/GetCategories`, `GET /PaymentMethods/GetPaymentMethods` (JWT, paginados)
  - `GET /Dashboard/Summary?year=&month=` (JWT)
  - `GET /Health/Health` (público)
- `expenseDate` es ISO DateTime; `amount` es number (decimal).

## Fases y estado

| # | Fase | Estado |
|---|---|---|
| 1 | Estructura base, environments, tokens, theme, httpClient/interceptors/guards, routing skeleton | **Completa + build validado** |
| 2 | Layouts (auth + authenticated) + componentes shared base (button, input, spinner, toast host, confirm-dialog, pagination, empty-state) | **Completa + tests/build validados** |
| 3 | Authentication (login, register, refresh flow) | **Completa + tests/build validados** |
| 4 | Dashboard (KPIs + comparativa barras + dona) | **Completa + tests/build validados + probado por el usuario** |
| 5 | Expenses (list, filtros, form-modal, confirm-delete, paginación) | **Completa + tests/build validados + probado por el usuario** |
| 6 | Categories + PaymentMethods | **Completa + tests/build validados** |
| 7 | Manejo global de errores/loading/feedback | Pendiente |
| 8 | Tests (auth.service, interceptors, guards, servicios, form-modal) | Pendiente |
| 9 | Revisión final + build | Pendiente |

## Comandos

```bash
cd frontend
npm install        # primera vez / cuando cambien dependencias
npm run build      # validar compilación
npm test           # tests (Vitest)
npm start          # dev server (puerto 4200 por defecto)
```

> El backend corre en `http://localhost:5003` (dev). CORS en el backend es permisiva (`PoliticaCors`), no requiere credenciales/cookies.

## Reglas del proceso

- Después de cada fase: `npm test` + `npm run build`, corregir errores, resumen de cambios.
- **No commits automáticos**: el usuario decide cuándo commitear (yo sugiero mensaje al final de cada fase).
- **No tocar el backend** sin aprobación previa del usuario.
- No inventar endpoints ni modelos: el backend es la fuente de verdad.
- No agregar dependencias sin justificar (solo `ng-apexcharts` + `apexcharts` aprobadas).
