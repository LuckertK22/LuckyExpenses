using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuckyExpenses.Migrations
{
    /// <inheritdoc />
    public partial class MakeCatalogsGlobal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_payment_methods_user_id",
                table: "payment_methods");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "payment_methods");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_name",
                table: "payment_methods",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_name",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "name", "icon", "created_at", "updated_at" },
                values: new object[,]
                {
                    { "Comida", "🍔", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Transporte", "🚗", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Vivienda", "🏠", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Servicios", "💡", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Salud", "🩺", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Educación", "📚", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Entretenimiento", "🎬", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Compras", "🛒", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Viajes", "✈️", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Otros", "📦", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "payment_methods",
                columns: new[] { "name", "created_at", "updated_at" },
                values: new object[,]
                {
                    { "Efectivo", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Tarjeta de débito", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Tarjeta de crédito", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Transferencia", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { "Otro", new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "payment_methods",
                keyColumn: "name",
                keyValues: new object[] { "Efectivo", "Tarjeta de débito", "Tarjeta de crédito", "Transferencia", "Otro" });

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "name",
                keyValues: new object[] { "Comida", "Transporte", "Vivienda", "Servicios", "Salud", "Educación", "Entretenimiento", "Compras", "Viajes", "Otros" });

            migrationBuilder.DropIndex(
                name: "IX_payment_methods_name",
                table: "payment_methods");

            migrationBuilder.DropIndex(
                name: "IX_categories_name",
                table: "categories");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "payment_methods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_user_id",
                table: "payment_methods",
                column: "user_id");
        }
    }
}
