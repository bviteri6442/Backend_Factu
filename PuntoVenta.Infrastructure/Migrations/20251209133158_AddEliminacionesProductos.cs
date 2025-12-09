using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PuntoVenta.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEliminacionesProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eliminaciones_productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoEliminadoId = table.Column<int>(type: "integer", nullable: false),
                    CodigoProductoEliminado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NombreProductoEliminado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescripcionProductoEliminado = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PrecioVentaProductoEliminado = table.Column<decimal>(type: "numeric", nullable: false),
                    PrecioCostoProductoEliminado = table.Column<decimal>(type: "numeric", nullable: false),
                    StockProductoEliminado = table.Column<int>(type: "integer", nullable: false),
                    AdministradorId = table.Column<int>(type: "integer", nullable: false),
                    NombreAdministrador = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FechaEliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotivoEliminacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TipoEliminacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eliminaciones_productos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eliminaciones_productos");
        }
    }
}
