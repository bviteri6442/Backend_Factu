using System;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Interfaz para implementar el patrón Unit of Work
    /// Proporciona acceso a todos los repositorios y maneja transacciones
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Productos { get; }
        IUsuarioRepository Usuarios { get; }
        IRolRepository Roles { get; }
        IClienteRepository Clientes { get; }
        IVentaRepository Ventas { get; }
        IErrorLogRepository ErrorLogs { get; }
        IIntentosLoginRepository IntentosLogin { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
