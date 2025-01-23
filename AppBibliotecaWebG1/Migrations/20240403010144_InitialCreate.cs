using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBibliotecaWebG1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    ISBN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Editorial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.ISBN);
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "ISBN", "Editorial", "Estado", "FechaPublicacion", "Foto", "PrecioVenta", "Titulo" },
                values: new object[] { 1, "Puntarenas", "A", new DateTime(2024, 4, 2, 19, 1, 44, 66, DateTimeKind.Local).AddTicks(4740), "ND", 27500m, "Lenguajes de programación" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
