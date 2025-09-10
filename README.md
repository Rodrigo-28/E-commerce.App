[README_ECommerce.md](https://github.com/user-attachments/files/22264977/README_ECommerce.md)
# E-commerce API — .NET 8 • Clean Architecture • SignalR • JWT

Backend de **e-commerce** construido con **.NET 8** siguiendo **Clean Architecture**.  
El sistema gestiona **catálogo**, **órdenes con cola de prioridad**, **notificaciones en tiempo real** y **autenticación JWT**, manteniendo un diseño limpio, testeable y escalable.

> Demo pensada para portafolio técnico: foco en arquitectura, reglas de negocio, procesamiento en segundo plano y comunicación en tiempo real.

---

## 🚀 Características principales

- **Catálogo de productos** con filtros por categoría, rango de precios, texto y “solo stock”.
- **Órdenes**: creación, cambio de estado y **procesamiento con prioridad** (exprés vs normal).
- **Cola de prioridad** en memoria usando `PriorityQueue<Order,int>`.
- **Procesamiento en segundo plano** con `BackgroundService` (decoupled del hilo web).
- **Notificaciones en tiempo real** vía **SignalR** al procesar órdenes.
- **Autenticación y autorización JWT** en API y en el Hub.
- **Validaciones** con FluentValidation y **mapeos** con AutoMapper.
- **Manejo de errores** centralizado (middleware) con respuestas JSON uniformes.
- **Swagger** para documentación y pruebas rápidas.

---

## 🧱 Arquitectura (Clean)

Capas concéntricas: dependencias **hacia adentro**.

- **Domain**: Entidades (`Product`, `Category`, `Order`), value objects, interfaces de repositorio.
- **Application**: Servicios de aplicación, **DTOs**, mapeos **AutoMapper**, reglas **FluentValidation**.
- **Infrastructure**: `DbContext` (**EF Core + Npgsql**), repositorios, servicio **JWT**, servicios externos; **BackgroundService** de procesamiento.
- **WebAPI (Presentation)**: Controladores REST, **Swagger**, middleware de excepciones y **SignalR Hub**.

Esta separación hace que la **lógica de negocio sea agnóstica** de base de datos, web o mensajería.

---

## 🧩 Casos de uso destacados

### A) Catálogo de productos
`GET /api/products` admite:
- `categoryId`, `minPrice`, `maxPrice`, `q` (búsqueda), `onlyStock=true/false`.

Flujo: Controller → Validator (FluentValidation) → `ProductService` → `ProductRepository` (LINQ/EF) → AutoMapper → DTO → JSON.  
**Índices** sugeridos: `(CategoryId)`, `(Price)`, `(Stock)`.

### B) Órdenes + Cola de prioridad
1. `POST /api/orders` crea la orden en **Draft**.  
2. `PUT /api/orders/{id}/status` con `Placed`:
   - `OrderService` actualiza estado y **encola** en `PriorityQueue<Order,int>`.
   - Prioridad: **0** = exprés, **1** = normal.
3. Un **BackgroundService** despierta cada _n_ segundos, **dequeue** priorizando exprés, procesa y deja listo para integrar pagos/logística.

### C) Notificaciones en tiempo real
Al procesar una orden:
```csharp
await Clients.All.SendAsync("OrderProcessed", new { id, status });
```
Front (demo simple `index.html`) se conecta con **SignalR** + JWT y muestra:  
`"Orden 23 procesada: Placed"` sin refrescar la página.

---

## 🔐 Seguridad, validaciones y errores

- **JWT Bearer** protege toda la API y el **Hub** (`[Authorize]`).  
- **FluentValidation** valida cada DTO (cantidades, existencia de productos, email, etc.).  
- **Middleware de excepciones** transforma errores (`NotFound`, `BadRequest`, `ValidationException`, `500`) en un **ErrorResponse JSON** consistente (código, tipo, mensaje).

---

## 🛠️ Tecnologías y librerías

Función | Librería
---|---
ORM & migraciones | **Entity Framework Core** + **Npgsql** → LINQ, migraciones y PostgreSQL
Mapeo DTO | **AutoMapper**
Validación | **FluentValidation**
Auth | **JWT Bearer** (Microsoft.AspNetCore.Authentication.JwtBearer)
Tiempo real | **SignalR**
Tareas BG | **BackgroundService** (in-proc)
Docs | **Swashbuckle / Swagger**

Referencias:  
- EF Core: https://learn.microsoft.com/ef/core/  
- Npgsql: https://www.npgsql.org/efcore/  
- AutoMapper: https://automapper.org/  
- FluentValidation: https://docs.fluentvalidation.net/  
- JWT Bearer: https://learn.microsoft.com/aspnet/core/security/authentication/jwt  
- SignalR: https://learn.microsoft.com/aspnet/core/signalr/introduction  
- Swagger/Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

---

## 📡 Endpoints (resumen)

### Products
- `GET /api/products` → filtros: `categoryId`, `minPrice`, `maxPrice`, `q`, `onlyStock`.
- `GET /api/products/{id}`
- `POST /api/products` · `PUT /api/products/{id}` · `DELETE /api/products/{id}`

### Orders
- `POST /api/orders` → crea **Draft**
- `PUT /api/orders/{id}/status` → `Draft → Placed` ⇒ **encolar**
- `GET /api/orders/{id}` / `GET /api/orders` (búsqueda/estado)

### Real-time
- `NotificationsHub` (SignalR) → evento: `"OrderProcessed"`  
  Conexión cliente: `/notifications?access_token={JWT}`  
  (hook vía `JwtBearerEvents.OnMessageReceived`)

---

## 🧵 Procesamiento en segundo plano (cómo funciona)

- **OrderQueue**: `PriorityQueue<Order,int>` con prioridad **0** (exprés) y **1** (normal).
- **OrderProcessingService** (`BackgroundService`):
  - Se despierta cada **5–10 s** (configurable).
  - `Dequeue()` siempre prioriza exprés.
  - Actualiza estado, escribe logs, emite SignalR (`OrderProcessed`).

> Para **una sola instancia** la cola en memoria es suficiente. En producción: **Redis/RabbitMQ** manteniendo la interfaz `IOrderQueue`.

---

## ⚙️ Configuración y ejecución

### Requisitos
- .NET 8
- PostgreSQL

### `appsettings.json` (ejemplo)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "tu conexion "
  },
  "Jwt": {
    "Key": "SuperSecretKey-256bits-ChangeMe",
    "Issuer": "ECommerce",
    "Audience": "ECommerceClient"
  }
}
```

### Migraciones y arranque
```bash
dotnet ef database update
dotnet run
```

### Swagger
```
https://localhost:<puerto>/swagger
```

---

## 🔎 Ejemplos rápidos

**Filtrar productos**
```
GET /api/products?categoryId=2&minPrice=100&q=monitor&onlyStock=true
Authorization: Bearer <token>
```

**Cambiar estado a Placed (encolar)**
```
PUT /api/orders/12/status
Body: { "status": "Placed", "isExpress": true }
Authorization: Bearer <token>
```

**Cliente (SignalR) — conexión con token**
```html
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"></script>
<script>
  const token = '<JWT>';
  const connection = new signalR.HubConnectionBuilder()
    .withUrl('/notifications?access_token=' + token)
    .build();

  connection.on('OrderProcessed', m => console.log(`Orden ${m.id} procesada: ${m.status}`));
  connection.start();
</script>
```

---

## 🧪 Testing

- **xUnit** (tests unitarios)
- **FakeItEasy** (dobles/mocks)
- **FluentAssertions** (aserciones expresivas)

Cobertura sugerida:
- **Repositorios**: CRUD y consultas con filtros (InMemory o Testcontainers).
- **Servicios**: cambio de estado + encolado correcto (exprés primero).
- **Controladores**: códigos HTTP y validaciones.

---

## 📈 Escalabilidad y próximos pasos

- Reemplazar cola en memoria por **Redis**/**RabbitMQ** manteniendo `IOrderQueue`.
- **Hangfire** para reintentos/telemetría de jobs.
- Cache con **Redis** para filtros de catálogo.
- Docker-compose: API + PostgreSQL + Redis.
- Observabilidad (logging estructurado, métricas, tracing).

---

## 🗂️ Estructura de carpetas (orientativa)

```
src/
├─ ECommerce.Domain/
├─ ECommerce.Application/
├─ ECommerce.Infrastructure/
└─ ECommerce.WebApi/
    ├─ Controllers/
    ├─ Middleware/
    ├─ Hubs/               # NotificationsHub (SignalR)
    └─ Program.cs
```

---

## 👤 Autor

**Rodrigo Romano** — Backend Developer  

