using PuntoVenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Interfaces
{
    /// <summary>
    /// Repository interface for Factura (Invoice) entity
    /// </summary>
    public interface IFacturaRepository : IGenericRepository<Factura>
    {
        Task<Factura?> GetFacturaConDetallesAsync(string id);
        Task<IEnumerable<Factura>> GetFacturasPorFechaAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<Factura>> GetFacturasPorUsuarioAsync(string usuarioId);
        Task<IEnumerable<Factura>> GetFacturasPorClienteAsync(string clienteId);
        Task<string> GenerarNumeroFacturaAsync();
        Task<Factura?> GetByNumeroFacturaAsync(string numeroFactura);
    }
}