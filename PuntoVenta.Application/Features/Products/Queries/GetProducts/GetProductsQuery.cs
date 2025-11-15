using MediatR;
using PuntoVenta.Application.DTOs;
using System.Collections.Generic;

namespace PuntoVenta.Application.Features.Products.Queries.GetProducts
{
	public class GetProductsQuery : IRequest<List<ProductoResponseDto>>
	{
		// Optionally add filters here later (search term, onlyWithStock, page, pageSize)
	}
}
