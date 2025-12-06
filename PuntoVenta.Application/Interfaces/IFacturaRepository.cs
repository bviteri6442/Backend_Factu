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
        Task<Factura?> GetFacturaConDetallesAsync(int id);
        Task<IEnumerable<Factura>> GetFacturasPorFechaAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<Factura>> GetFacturasPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Factura>> GetFacturasPorClienteAsync(int clienteId);
        Task<string> GenerarNumeroFacturaAsync();
        Task<Factura?> GetByNumeroFacturaAsync(string numeroFactura);
    }
}