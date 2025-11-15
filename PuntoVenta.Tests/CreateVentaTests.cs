using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PuntoVenta.Infrastructure.Persistencia;
using PuntoVenta.Infrastructure.Repositories;
using Xunit;
using PuntoVenta.Application.Features.Ventas.Commands;
using PuntoVenta.Application.DTOs;
using PuntoVenta.Domain.Entities;
using System.Collections.Generic;

namespace PuntoVenta.Tests
{
    public class CreateVentaTests
    {
        private ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateVenta_ReducesStockAndCreatesVenta()
        {
            using var context = CreateContext();

            var product = new Product
            {
                Nombre = "Producto Test",
                CodigoBarra = "12345",
                PrecioVenta = 100m,
                PrecioCosto = 60m,
                StockActual = 10
            };

            context.Productos.Add(product);
            await context.SaveChangesAsync();

            var uow = new UnitOfWork(context);
            var handler = new CreateVentaCommandHandler(uow);

            var command = new CreateVentaCommand
            {
                UsuarioId = "test-user",
                Detalles = new List<CreateDetalleVentaDto>
                {
                    new CreateDetalleVentaDto { ProductoId = product.Id, Cantidad = 3, PrecioUnitario = 100m }
                }
            };

            var ventaId = await handler.Handle(command, default);

            Assert.True(ventaId > 0);

            var productoActualizado = await context.Productos.FindAsync(product.Id);
            Assert.Equal(7, productoActualizado.StockActual);
        }

        [Fact]
        public async Task CreateVenta_ThrowsWhenInsufficientStock()
        {
            using var context = CreateContext();

            var product = new Product
            {
                Nombre = "Producto Poco Stock",
                CodigoBarra = "54321",
                PrecioVenta = 50m,
                PrecioCosto = 30m,
                StockActual = 1
            };

            context.Productos.Add(product);
            await context.SaveChangesAsync();

            var uow = new UnitOfWork(context);
            var handler = new CreateVentaCommandHandler(uow);

            var command = new CreateVentaCommand
            {
                UsuarioId = "test-user",
                Detalles = new List<CreateDetalleVentaDto>
                {
                    new CreateDetalleVentaDto { ProductoId = product.Id, Cantidad = 5, PrecioUnitario = 50m }
                }
            };

            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(command, default));
        }
    }
}
