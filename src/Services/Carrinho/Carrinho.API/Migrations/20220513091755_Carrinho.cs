using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carrinho.API.Migrations
{
    public partial class Carrinho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarrinhoClientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoClientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarrinhoItens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    Imagem = table.Column<string>(type: "varchar(100)", nullable: false),
                    CarrinhoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrinhoItens_CarrinhoClientes_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalTable: "CarrinhoClientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CLiente",
                table: "CarrinhoClientes",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItens_CarrinhoId",
                table: "CarrinhoItens",
                column: "CarrinhoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrinhoItens");

            migrationBuilder.DropTable(
                name: "CarrinhoClientes");
        }
    }
}
