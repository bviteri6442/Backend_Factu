using MediatR;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PuntoVenta.Application.Features.Products.Queries.GetProducts
{
	public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductoResponseDto>>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetProductsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<List<ProductoResponseDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var productos = await _unitOfWork.Productos.GetAllAsync();

			return productos.Select(p => new ProductoResponseDto
			{
				Id = p.Id,
				CodigoBarra = p.CodigoBarra,
				Nombre = p.Nombre,
				PrecioCosto = p.PrecioCosto,
				PrecioVenta = p.PrecioVenta,
				StockActual = p.StockActual,
				StockMinimo = p.StockMinimo,
				Descripcion = p.Descripcion,
				FechaCreacion = p.FechaCreacion,
				FechaActualizacion = p.FechaActualizacion,
				Activo = p.Activo
			}).ToList();
		}
	}
}
