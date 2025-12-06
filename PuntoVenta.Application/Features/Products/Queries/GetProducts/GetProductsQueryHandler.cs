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
				Codigo = p.Codigo,
				Nombre = p.Nombre,
				PrecioCompra = p.PrecioCompra,
				Precio = p.Precio,
				Stock = p.Stock,
				StockMinimo = p.StockMinimo,
				Descripcion = p.Descripcion,
				FechaCreacion = p.FechaCreacion,
				FechaActualizacion = p.FechaActualizacion ?? p.FechaCreacion,
				Activo = p.Activo
			}).ToList();
		}
	}
}
