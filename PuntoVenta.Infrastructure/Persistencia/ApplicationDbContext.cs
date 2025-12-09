using Microsoft.EntityFrameworkCore;
using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Infrastructure.Persistencia
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Product> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<IntentosLogin> IntentosLogin { get; set; }
        public DbSet<EliminacionUsuario> EliminacionesUsuarios { get; set; }
        public DbSet<EliminacionProducto> EliminacionesProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Documento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Direccion).HasMaxLength(300);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.Documento).IsUnique();
            });

            // Configuración de Producto
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("productos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PrecioCompra).HasColumnType("decimal(18,2)");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.Codigo).IsUnique();
            });

            // Configuración de Rol
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(300);
                
                entity.HasIndex(e => e.Nombre).IsUnique();
            });

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.NombreUsuario).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                
                // Relación con Rol
                entity.HasOne<Rol>()
                    .WithMany()
                    .HasForeignKey(e => e.RolId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Factura
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("facturas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NumeroFactura).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UsuarioNombre).HasMaxLength(200);
                entity.Property(e => e.ClienteNombre).HasMaxLength(200);
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PorcentajeIVA).HasColumnType("decimal(5,2)");
                entity.Property(e => e.TotalImpuesto).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalVenta).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.FechaVenta).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.NumeroFactura).IsUnique();
                
                // Relación con Usuario
                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Relación con Cliente (opcional)
                entity.HasOne<Cliente>()
                    .WithMany()
                    .HasForeignKey(e => e.ClienteId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
                
                // Relación con DetallesVenta
                entity.HasMany(e => e.Detalles)
                    .WithOne()
                    .HasForeignKey("FacturaId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de DetalleVenta
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("detalles_venta");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ProductoNombre).HasMaxLength(200);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descuento).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
                
                // Relación con Producto
                entity.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(e => e.ProductoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de ErrorLog
            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.ToTable("error_logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Mensaje).IsRequired();
                entity.Property(e => e.Nivel).HasMaxLength(50);
                entity.Property(e => e.UsuarioId).HasMaxLength(50);
                entity.Property(e => e.Origen).HasMaxLength(200);
                entity.Property(e => e.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configuración de IntentosLogin
            modelBuilder.Entity<IntentosLogin>(entity =>
            {
                entity.ToTable("intentos_login");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.FechaIntento).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.NombreUsuario);
            });

            // Configuración de EliminacionUsuario
            modelBuilder.Entity<EliminacionUsuario>(entity =>
            {
                entity.ToTable("eliminaciones_usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CedulaUsuarioEliminado).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreUsuarioEliminado).IsRequired().HasMaxLength(200);
                entity.Property(e => e.EmailUsuarioEliminado).IsRequired().HasMaxLength(100);
                entity.Property(e => e.RolUsuarioEliminado).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreAdministrador).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MotivoEliminacion).HasMaxLength(500);
                entity.Property(e => e.TipoEliminacion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.FechaEliminacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasIndex(e => e.FechaEliminacion);
                entity.HasIndex(e => e.CedulaUsuarioEliminado);
            });
        }
    }
}
