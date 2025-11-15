# PuntoVenta - Sistema de Punto de Venta (POS)

Una solución backend profesional para un sistema de punto de venta construida con **ASP.NET Core 8**, **Entity Framework Core 8** y **SQL Server**.

## 📋 Características principales

- ✅ **Gestión de usuarios y roles** — Autenticación JWT, control de acceso basado en roles (RBAC)
- ✅ **Ventas completas (CRUD)** — Crear, listar, actualizar y eliminar órdenes de venta
- ✅ **Control de inventario** — Validación de stock, decremento automático, alertas de bajo stock
- ✅ **Gestión de clientes** — CRUD de clientes con información de contacto
- ✅ **Gestión de productos** — Catálogo de productos con precios y códigos de barras
- ✅ **Gestión de errores centralizada** — Middleware global de excepciones con registro en BD
- ✅ **Transacciones ACID** — Operaciones críticas (ventas) con rollback automático
- ✅ **API REST con Swagger** — Documentación interactiva y fácil de usar

## 🛠️ Stack tecnológico

| Componente | Tecnología | Versión |
|------------|-----------|---------|
| Framework | ASP.NET Core | 8.0 |
| ORM | Entity Framework Core | 8.0 |
| BD | SQL Server | LocalDB / Express / Cloud |
| Autenticación | JWT (Json Web Tokens) | - |
| Hash de contraseñas | BCrypt.Net-Next | 4.0.3 |
| Patrón de aplicación | CQRS (MediatR) | 11.1.0 |
| Mapeo de objetos | AutoMapper | 12.0.1 |
| Validación | FluentValidation | 11.9.1 |
| Documentación API | Swagger (Swashbuckle) | 6.4.0 |

## 📁 Estructura del proyecto

```
PuntoVenta/
├── PuntoVenta.Domain/              # Capa de dominio (entidades, interfaces)
│   └── Entities/
│       ├── Usuario.cs
│       ├── Rol.cs
│       ├── Product.cs
│       ├── Cliente.cs
│       ├── Venta.cs
│       ├── DetalleVenta.cs
│       ├── ErrorLog.cs
│       └── IntentosLogin.cs
├── PuntoVenta.Application/         # Capa de aplicación (DTOs, casos de uso)
│   ├── DTOs/
│   ├── Features/
│   │   ├── Usuarios/
│   │   ├── Ventas/
│   │   ├── Products/
│   │   └── Clientes/
│   └── Interfaces/
├── PuntoVenta.Infrastructure/      # Capa de infraestructura (BD, repositorios)
│   ├── Persistencia/
│   ├── Repositories/
│   └── Migrations/
├── PuntoVenta.Api/                 # API (controladores, middleware)
│   ├── Controllers/
│   ├── Middleware/
│   ├── Extensions/
│   ├── Program.cs
│   └── appsettings.json
└── PuntoVenta.Tests/               # Pruebas unitarias e integración
```

## 🚀 Guía de inicio rápido

### Requisitos previos

- **.NET 8 SDK** — [Descargar](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** — SQL Server Express, LocalDB o versión completa
- **Visual Studio Code** o **Visual Studio 2022** (opcional)

### Configuración inicial

#### 1. Clonar el repositorio y restaurar paquetes

```bash
cd "C:\Users\Usuario\Desktop\Proyectos WEB\ProyectoWEB"
dotnet restore
```

#### 2. Configurar la base de datos

Editar `PuntoVenta.Api/appsettings.json` y establecer la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PuntoVentaDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "TuClaveSuperSecretaDeMasDeReiintaDosCaracteresAquiMismoOk123456789"
  }
}
```

#### 3. Crear y aplicar migraciones

```bash
cd "c:\Users\Usuario\Desktop\Proyectos WEB\ProyectoWEB\PuntoVenta.Infrastructure"
dotnet ef migrations add InitialMigration --startup-project ../PuntoVenta.Api
dotnet ef database update --startup-project ../PuntoVenta.Api
```

#### 4. Ejecutar la aplicación

```bash
cd "c:\Users\Usuario\Desktop\Proyectos WEB\ProyectoWEB"
dotnet build
dotnet run --project PuntoVenta.Api
```

La API estará disponible en `https://localhost:5001` (o el puerto configurado).

#### 5. Acceder a Swagger

Abrir navegador en: `https://localhost:5001/swagger`

## 📚 Endpoints principales

### Autenticación

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| POST | `/api/auth/login` | Login de usuario | No |
| POST | `/api/auth/logout` | Logout de usuario | Sí (JWT) |

### Usuarios

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/usuarios` | Listar usuarios | Sí |
| GET | `/api/usuarios/{id}` | Obtener usuario por ID | Sí |
| POST | `/api/usuarios` | Crear usuario | Sí |
| PUT | `/api/usuarios/{id}` | Actualizar usuario | Sí |
| DELETE | `/api/usuarios/{id}` | Eliminar usuario | Sí |

### Productos

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/productos` | Listar productos con paginación | Sí |
| GET | `/api/productos/{id}` | Obtener producto por ID | Sí |
| POST | `/api/productos` | Crear producto | Sí |
| PUT | `/api/productos/{id}` | Actualizar producto | Sí |
| DELETE | `/api/productos/{id}` | Eliminar producto | Sí |

### Clientes

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/clientes` | Listar clientes | Sí |
| GET | `/api/clientes/{id}` | Obtener cliente por ID | Sí |
| POST | `/api/clientes` | Crear cliente | Sí |
| PUT | `/api/clientes/{id}` | Actualizar cliente | Sí |
| DELETE | `/api/clientes/{id}` | Eliminar cliente | Sí |

### Ventas (Órdenes)

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/ventas` | Listar ventas con paginación y filtros | Sí |
| GET | `/api/ventas/{id}` | Obtener detalle de venta | Sí |
| POST | `/api/ventas` | Crear nueva venta | Sí |
| PUT | `/api/ventas/{id}` | Actualizar estado/observaciones | Sí |
| DELETE | `/api/ventas/{id}` | Eliminar venta (restaura stock) | Sí |

### Roles

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/roles` | Listar roles | Sí |
| POST | `/api/roles` | Crear rol | Sí |
| PUT | `/api/roles/{id}` | Actualizar rol | Sí |
| DELETE | `/api/roles/{id}` | Eliminar rol | Sí |

### Errores

| Método | Endpoint | Descripción | Autenticación |
|--------|----------|-------------|----------------|
| GET | `/api/error-logs` | Listar errores (solo Admin) | Sí |
| GET | `/api/error-logs/{id}` | Obtener error por ID | Sí |
| PUT | `/api/error-logs/{id}/mark-reviewed` | Marcar error como revisado | Sí |

## 🔐 Seguridad

### Autenticación JWT

- **Token:** Almacenado en header `Authorization: Bearer <token>`
- **Duración:** Aproximadamente 24 horas (configurable)
- **Algoritmo:** HMAC-SHA256

#### Ejemplo de login:

```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "correo": "usuario@example.com",
    "contrasena": "Password123!"
  }'
```

Respuesta:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": { "id": "1", "correo": "usuario@example.com" }
}
```

### Control de acceso (RBAC)

Los endpoints están protegidos por roles. Los roles disponibles son:

- **Administrador** — Acceso total a todas las operaciones
- **Vendedor** — Crear/gestionar ventas y ver productos
- **Gerente** — Ver reportes y gestionar inventario
- **Cliente** — Acceso limitado a su información

### Bloqueo de cuenta

Después de 3 intentos fallidos de login, la cuenta se bloquea automáticamente por 15 minutos.

## 💾 Variables de entorno

Crear un archivo `.env` en la raíz del proyecto (o configurar en appsettings):

```env
# Base de datos
CONNECTION_STRING=Server=(localdb)\\mssqllocaldb;Database=PuntoVentaDb;Trusted_Connection=true;

# JWT
JWT_SECRET_KEY=TuClaveSuperSecretaDeMasDeReiintaDosCaracteresAquiMismoOk123456789
JWT_EXPIRATION_HOURS=24

# CORS (producción)
ALLOWED_ORIGINS=https://tudominio.com,https://www.tudominio.com

# Rate Limiting
MAX_REQUESTS_PER_MINUTE=100
```

## 🧪 Ejecución de pruebas

```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas de un proyecto específico
dotnet test PuntoVenta.Tests

# Con cobertura
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## 📊 Paginación y filtros

### GetVentas con paginación

```bash
GET /api/ventas?pageNumber=1&pageSize=10&sortBy=FechaVenta&descending=true&searchTerm=cliente
```

Parámetros:
- `pageNumber` (int, default: 1) — Número de página
- `pageSize` (int, default: 10) — Tamaño de página
- `sortBy` (string) — Campo para ordenar (FechaVenta, TotalVenta, NumeroFactura)
- `descending` (bool, default: true) — Orden descendente
- `searchTerm` (string) — Búsqueda por cliente, factura, etc.

Respuesta:
```json
{
  "data": [...],
  "totalCount": 150,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 15
}
```

## 🌱 Seed Data (Datos de prueba)

Para generar datos de prueba masivos (opcional):

```bash
dotnet run --project PuntoVenta.Api -- --seed
```

Esto creará:
- 50 usuarios con roles diversos
- 100 productos con stock e información
- 500 clientes
- 1,000 ventas históricas

## 🚢 Despliegue

### Azure App Service + Azure SQL

1. **Crear recursos en Azure:**
   ```bash
   az group create --name PuntoVenta-rg --location eastus
   az appservice plan create --name PuntoVenta-plan --resource-group PuntoVenta-rg --sku B2
   az webapp create --resource-group PuntoVenta-rg --plan PuntoVenta-plan --name puntoventa-api
   ```

2. **Publicar la API:**
   ```bash
   dotnet publish -c Release -o ./publish
   cd publish
   Compress-Archive -Path * -DestinationPath ..\puntoventa-api.zip
   az webapp deployment source config-zip --resource-group PuntoVenta-rg --name puntoventa-api --src ..\puntoventa-api.zip
   ```

3. **Configurar variables de entorno en Azure Portal** — Establecer `CONNECTION_STRING` y `JWT_SECRET_KEY`.

### Docker

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "PuntoVenta.Api.dll"]
```

Construir e ejecutar:
```bash
docker build -t puntoventa-api .
docker run -p 5001:443 -e CONNECTION_STRING="..." puntoventa-api
```

## 📖 Documentación adicional

- **Entity Framework Core Migrations:** [Guía oficial](https://learn.microsoft.com/es-es/ef/core/managing-schemas/migrations/)
- **JWT en ASP.NET Core:** [Implementación](https://learn.microsoft.com/es-es/aspnet/core/security/authentication/jwt-authn)
- **CQRS y MediatR:** [Patrón CQRS](https://learn.microsoft.com/es-es/azure/architecture/patterns/cqrs)

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:
1. Fork el proyecto
2. Crea una rama (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo licencia MIT. Ver `LICENSE` para más detalles.

## 📞 Soporte

Para reportar bugs o solicitar características, abrir un **Issue** en el repositorio o contactar al equipo de desarrollo.

---

**Última actualización:** 15 de noviembre de 2025  
**Versión:** 1.0.0
