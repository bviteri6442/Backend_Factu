using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PuntoVenta.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEliminacionesUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eliminaciones_usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioEliminadoId = table.Column<int>(type: "integer", nullable: false),
                    CedulaUsuarioEliminado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NombreUsuarioEliminado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmailUsuarioEliminado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RolUsuarioEliminado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AdministradorId = table.Column<int>(type: "integer", nullable: false),
                    NombreAdministrador = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FechaEliminacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MotivoEliminacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TipoEliminacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eliminaciones_usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eliminaciones_usuarios_CedulaUsuarioEliminado",
                table: "eliminaciones_usuarios",
                column: "CedulaUsuarioEliminado");

            migrationBuilder.CreateIndex(
                name: "IX_eliminaciones_usuarios_FechaEliminacion",
                table: "eliminaciones_usuarios",
                column: "FechaEliminacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eliminaciones_usuarios");
        }
    }
}
