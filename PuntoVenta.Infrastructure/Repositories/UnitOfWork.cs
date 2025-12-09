using Microsoft.EntityFrameworkCore.Storage;
using PuntoVenta.Application.Interfaces;
using PuntoVenta.Infrastructure.Persistencia;
using System;
using System.Threading.Tasks;

namespace PuntoVenta.Infrastructure.Repositories
{
    /// <summary>
    /// EF Core Unit of Work implementation with transaction support
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        private IProductRepository? _productRepository;
        private IUsuarioRepository? _usuarioRepository;
        private IRolRepository? _rolRepository;
        private IClienteRepository? _clienteRepository;
        private IFacturaRepository? _facturaRepository;
        private IErrorLogRepository? _errorLogRepository;
        private IIntentosLoginRepository? _intentosLoginRepository;
        private IEliminacionUsuarioRepository? _eliminacionUsuarioRepository;
        private IEliminacionProductoRepository? _eliminacionProductoRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IProductRepository Productos
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }

        public IUsuarioRepository Usuarios
        {
            get
            {
                return _usuarioRepository ??= new UsuarioRepository(_context);
            }
        }

        public IRolRepository Roles
        {
            get
            {
                return _rolRepository ??= new RolRepository(_context);
            }
        }

        public IClienteRepository Clientes
        {
            get
            {
                return _clienteRepository ??= new ClienteRepository(_context);
            }
        }

        public IFacturaRepository Facturas
        {
            get
            {
                return _facturaRepository ??= new FacturaRepository(_context);
            }
        }

        public IErrorLogRepository ErrorLogs
        {
            get
            {
                return _errorLogRepository ??= new ErrorLogRepository(_context);
            }
        }

        public IIntentosLoginRepository IntentosLogin
        {
            get
            {
                return _intentosLoginRepository ??= new IntentosLoginRepository(_context);
            }
        }

        public IEliminacionUsuarioRepository EliminacionesUsuarios
        {
            get
            {
                return _eliminacionUsuarioRepository ??= new EliminacionUsuarioRepository(_context);
            }
        }

        public IEliminacionProductoRepository EliminacionesProductos
        {
            get
            {
                return _eliminacionProductoRepository ??= new EliminacionProductoRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
