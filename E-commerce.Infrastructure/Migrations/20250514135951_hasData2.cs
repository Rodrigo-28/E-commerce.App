using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class hasData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "IsExpress", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 14, 10, 30, 0, 0, DateTimeKind.Utc), false, 1, 1 },
                    { 2, new DateTime(2025, 5, 15, 14, 45, 0, 0, DateTimeKind.Utc), true, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Smartphone X1", 699.99m, 25 },
                    { 2, 1, "Televisor 4K 55”", 499.50m, 10 },
                    { 3, 2, "Camiseta Deportiva", 29.99m, 100 },
                    { 4, 3, "Juego de Sábanas King", 59.90m, 40 },
                    { 5, 4, "Pelota de Fútbol Oficial", 19.99m, 60 },
                    { 6, 5, "Novela “El Misterio”", 14.95m, 80 },
                    { 7, 6, "Set Maquillaje Profesional", 49.00m, 35 },
                    { 8, 7, "Lego Castillo Medieval", 89.99m, 20 },
                    { 9, 8, "Aceite Motor 5W-30", 24.50m, 30 },
                    { 10, 9, "Silla Plegable Jardín", 79.99m, 15 },
                    { 11, 10, "Pulsera Inteligente Fit", 129.99m, 22 }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 699.99m },
                    { 2, 1, 6, 2, 14.95m },
                    { 3, 2, 3, 3, 29.99m },
                    { 4, 2, 10, 1, 79.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
