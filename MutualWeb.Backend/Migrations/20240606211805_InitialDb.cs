﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MutualWeb.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionTipoCliente = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ClienteInae = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoClientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoClientes_DescripcionTipoCliente",
                table: "TipoClientes",
                column: "DescripcionTipoCliente",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoClientes");
        }
    }
}
