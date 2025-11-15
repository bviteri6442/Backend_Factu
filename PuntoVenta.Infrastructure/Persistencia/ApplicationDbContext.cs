using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Infrastructure.Persistencia
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<IntentosLogin> IntentosLogin { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuración de DetalleVenta - clave compuesta
            modelBuilder.Entity<DetalleVenta>()
                .HasKey(dv => new { dv.VentaId, dv.ProductoId });
            
            // Índice único para Código de Barras
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CodigoBarra)
                .IsUnique();
            
            // Índice único para Cédula de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Cedula)
                .IsUnique();
            
            // Índice único para Correo de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();
            
            // Índice único para Documento de Cliente
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Documento)
                .IsUnique();
            
            // Relación Rol - Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relación Venta - Cliente
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Relación DetalleVenta - Venta
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(dv => dv.VentaId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relación DetalleVenta - Producto
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany()
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Índice para búsquedas por correo en IntentosLogin
            modelBuilder.Entity<IntentosLogin>()
                .HasIndex(il => il.Correo);
        }
    }
}
