using PuntoVenta.Domain.Entities;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for IntentosLogin entity
    /// </summary>
    public interface IIntentosLoginRepository : IGenericRepository<IntentosLogin>
    {
        Task<IntentosLogin?> GetByCorreoAsync(string correo);
        Task IncrementarIntentosAsync(string correo, string ip, string userAgent);
        Task ReiniciarIntentosAsync(string correo);
        Task BloquearUsuarioAsync(string correo);
    }
}