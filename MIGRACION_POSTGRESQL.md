# Guía de Migración de MongoDB a PostgreSQL

## ✅ COMPLETADO

### 1. Paquetes NuGet Instalados
- ✅ Npgsql.EntityFrameworkCore.PostgreSQL v8.0.0
- ✅ Microsoft.EntityFrameworkCore.Design v8.0.0

### 2. Entidades Actualizadas
Todas las entidades han sido migradas de MongoDB a PostgreSQL:
- ✅ Cliente: `string Id` → `int Id`
- ✅ Product: Actualizado propiedades (CodigoBarra → Codigo, PrecioVenta → Precio, etc.)
- ✅ Usuario: Actualizado propiedades (Cedula → NombreUsuario, Correo → Email, etc.)
- ✅ Rol: Actualizado con int Id y soporte JSON para Permisos
- ✅ Factura: Actualizado relaciones y IDs
- ✅ DetalleVenta: Actualizado con FK a Factura
- ✅ ErrorLog: Simplificado para PostgreSQL
- ✅ IntentosLogin: Actualizado estructura

### 3. DbContext Creado
- ✅ ApplicationDbContext.cs con todas las configuraciones de entidades
- ✅ Relaciones configuradas correctamente
- ✅ Índices únicos definidos

### 4. Configuration actualizada
- ✅ appsettings.json modificado con connection string de PostgreSQL

## 📋 PENDIENTE - NECESITAS COMPLETAR

### Paso 1: Instalar PostgreSQL
```powershell
# Opción 1: Descargar desde https://www.postgresql.org/download/windows/
# Opción 2: Usar Docker
docker run --name postgres-puntoventa -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:16
```

**Credenciales por defecto (puedes cambiarlas en appsettings.json):**
- Host: localhost
- Port: 5432
- Database: PuntoVentaDb
- Username: postgres
- Password: postgres

### Paso 2: Actualizar Program.cs y ServiceCollectionExtensions
Necesitas reemplazar la configuración de MongoDB por Entity Framework Core.

**Archivo a modificar: `PuntoVenta.Infrastructure\ServiceCollectionExtensions.cs`**

Cambiar esto:
```csharp
// MongoDB configuration
services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
services.AddSingleton<MongoDbContext>();
```

Por esto:
```csharp
// PostgreSQL configuration
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
```

**Archivo a modificar: `PuntoVenta.Api\Program.cs`**

Busca y elimina referencias a MongoDB, asegúrate que use Entity Framework Core.

### Paso 3: Crear Migraciones de Entity Framework Core
```powershell
cd "c:\Users\LEGIO\Documents\QUINTO\TECNOLO. Y DESA. WEB\PFINAL\Backend_Factu"

# Crear migración inicial
dotnet ef migrations add InitialCreate --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api

# Aplicar migración a la base de datos
dotnet ef database update --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api
```

### Paso 4: Actualizar Repositorios
Los repositorios actuales usan MongoDB Driver. Necesitas crear nuevos repositorios con Entity Framework Core.

**Crear: `PuntoVenta.Infrastructure\Repositories\ClienteRepository.cs`**
```csharp
using Microsoft.EntityFrameworkCore;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;

namespace PuntoVenta.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Documento == documento);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;
            
            cliente.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

Replica este patrón para:
- ProductRepository
- UsuarioRepository
- RolRepository
- FacturaRepository
- ErrorLogRepository
- IntentosLoginRepository

### Paso 5: Actualizar DTOs
Los DTOs actualmente usan `string` para IDs. Necesitas cambiarlos a `int`:

**Archivos a modificar:**
- `ClienteDto.cs`: `int ClienteId` (en lugar de string)
- `ProductoDto.cs`: `int ProductoId`
- `UsuarioDto.cs`: `int UsuarioId`
- `VentaDto.cs`: `int VentaId`, `int? ClienteId`, `int UsuarioId`

### Paso 6: Actualizar Controllers
Los controllers usan IDs como string. Necesitas cambiar signatures:

Ejemplo: `ClientesController.cs`
```csharp
[HttpGet("{id}")]
public async Task<ActionResult<ClienteDto>> GetById(int id)  // era string id

[HttpPut("{id}")]
public async Task<ActionResult> Update(int id, UpdateClienteDto dto)  // era string id

[HttpDelete("{id}")]
public async Task<ActionResult> Delete(int id)  // era string id
```

### Paso 7: Seed Data (Datos iniciales)
Crea datos iniciales para poder probar el sistema:

**Crear: `PuntoVenta.Infrastructure\Data\DbInitializer.cs`**
```csharp
using PuntoVenta.Domain.Entities;
using PuntoVenta.Infrastructure.Persistencia;
using BCrypt.Net;

namespace PuntoVenta.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // Si ya hay datos, salir
            if (context.Roles.Any()) return;

            // Crear Rol Administrador
            var rolAdmin = new Rol
            {
                Nombre = "Administrador",
                Descripcion = "Acceso total al sistema",
                Permisos = new List<string> { "*" }
            };
            context.Roles.Add(rolAdmin);
            await context.SaveChangesAsync();

            // Crear Usuario Administrador
            var adminUser = new Usuario
            {
                NombreUsuario = "admin",
                Email = "bryanyb2010@gmail.com",
                Nombre = "Administrador Sistema",
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123!", HashType.SHA384),
                RolId = rolAdmin.Id,
                RolNombre = "Administrador"
            };
            context.Usuarios.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
```

Llama a este inicializador en `Program.cs`:
```csharp
// Después de app.Build() y antes de app.Run()
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.Initialize(context);
}
```

### Paso 8: Migrar Datos de MongoDB a PostgreSQL (OPCIONAL)
Si tienes datos importantes en MongoDB:

**Crear script: `MigracionDatos.cs`**
```csharp
// Conectar a ambas bases de datos y copiar registros
// Esto es opcional si quieres preservar datos existentes
```

## 🔧 COMANDOS ÚTILES

### Ver Migraciones
```powershell
dotnet ef migrations list --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api
```

### Eliminar Última Migración
```powershell
dotnet ef migrations remove --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api
```

### Actualizar Base de Datos
```powershell
dotnet ef database update --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api
```

### Generar Script SQL
```powershell
dotnet ef migrations script --project PuntoVenta.Infrastructure --startup-project PuntoVenta.Api --output migration.sql
```

## ⚠️ ADVERTENCIAS

1. **Backup de MongoDB**: Haz un respaldo de tu base de datos MongoDB actual antes de migrar.
2. **Testing**: Prueba todo en un entorno de desarrollo antes de producción.
3. **Cambios en Frontend**: El frontend NO necesita cambios si mantienes los mismos endpoints y DTOs.
4. **IDs**: Los IDs cambiarán de `string` a `int`, asegúrate de actualizar todas las referencias.

## 📝 CHECKLIST FINAL

- [ ] PostgreSQL instalado y corriendo
- [ ] appsettings.json actualizado con connection string correcta
- [ ] ServiceCollectionExtensions actualizado para EF Core
- [ ] Migraciones creadas y aplicadas
- [ ] Repositorios actualizados para EF Core
- [ ] DTOs actualizados (string → int para IDs)
- [ ] Controllers actualizados (signatures con int)
- [ ] Seed data ejecutado
- [ ] Backend compilando sin errores
- [ ] Tests básicos funcionando (login, crear cliente, crear producto)

## 🚀 PRÓXIMOS PASOS DESPUÉS DE COMPLETAR

1. Ejecuta `dotnet build` para verificar que todo compila
2. Ejecuta `dotnet run` para iniciar el backend
3. Prueba login con usuario admin
4. Prueba crear un cliente, producto y factura
5. Verifica que los PDFs sigan funcionando

¿Necesitas ayuda con algún paso específico?
