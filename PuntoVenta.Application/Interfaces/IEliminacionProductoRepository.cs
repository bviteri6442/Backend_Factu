using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Application.Interfaces
{
    public interface IEliminacionProductoRepository : IGenericRepository<EliminacionProducto>
    {
        Task<IEnumerable<EliminacionProducto>> BuscarAsync(string searchTerm);
    }
}
