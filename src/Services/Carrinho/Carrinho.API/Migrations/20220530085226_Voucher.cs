using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carrinho.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CarrinhoId",
                table: "CarrinhoItens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "Desconto",
                table: "CarrinhoClientes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentual",
                table: "CarrinhoClientes",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoDesconto",
                table: "CarrinhoClientes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "CarrinhoClientes",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCodigo",
                table: "CarrinhoClientes",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "VoucherUtilizado",
                table: "CarrinhoClientes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desconto",
                table: "CarrinhoClientes");

            migrationBuilder.DropColumn(
                name: "Percentual",
                table: "CarrinhoClientes");

            migrationBuilder.DropColumn(
                name: "TipoDesconto",
                table: "CarrinhoClientes");

            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "CarrinhoClientes");

            migrationBuilder.DropColumn(
                name: "VoucherCodigo",
                table: "CarrinhoClientes");

            migrationBuilder.DropColumn(
                name: "VoucherUtilizado",
                table: "CarrinhoClientes");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarrinhoId",
                table: "CarrinhoItens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
