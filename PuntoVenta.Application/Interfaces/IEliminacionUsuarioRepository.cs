using PuntoVenta.Domain.Entities;

namespace PuntoVenta.Application.Interfaces
{
    public interface IEliminacionUsuarioRepository : IGenericRepository<EliminacionUsuario>
    {
        Task<IEnumerable<EliminacionUsuario>> BuscarAsync(string termino);
        Task<IEnumerable<EliminacionUsuario>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta);
    }
}
