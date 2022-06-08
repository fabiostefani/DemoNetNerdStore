using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carrinho.API.Migrations
{
    public partial class Configdeletecarrinho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItens_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItens");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItens_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItens",
                column: "CarrinhoId",
                principalTable: "CarrinhoClientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItens_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItens");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItens_CarrinhoClientes_CarrinhoId",
                table: "CarrinhoItens",
                column: "CarrinhoId",
                principalTable: "CarrinhoClientes",
                principalColumn: "Id");
        }
    }
}
