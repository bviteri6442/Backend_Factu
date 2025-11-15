using MediatR;
using PuntoVenta.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Ventas.Commands
{
    public class DeleteVentaCommandHandler : IRequestHandler<DeleteVentaCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVentaCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteVentaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var venta = await _unitOfWork.Ventas.GetByIdAsync(request.VentaId);

                if (venta == null)
                {
                    throw new Exception($"Venta con ID {request.VentaId} no encontrada");
                }

                // Solo permitir eliminar ventas en estado Cancelada o completadas muy antiguas
                if (venta.Estado != "Cancelada" && venta.Estado != "Anulada")
                {
                    throw new Exception($"No se puede eliminar una venta en estado {venta.Estado}");
                }

                // Restaurar stock de productos si se elimina una venta
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _unitOfWork.Productos.GetByIdAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.StockActual += detalle.Cantidad;
                        await _unitOfWork.Productos.UpdateAsync(producto);
                    }
                }

                await _unitOfWork.Ventas.DeleteAsync(request.VentaId);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar venta: {ex.Message}");
            }
        }
    }
}
