using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuckyExpenses.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "categories");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "payment_methods",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "categories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "categories" SET "code" = CASE "name"
                    WHEN 'Comida' THEN 'FOOD'
                    WHEN 'Transporte' THEN 'TRANSPORT'
                    WHEN 'Vivienda' THEN 'HOUSING'
                    WHEN 'Servicios' THEN 'UTILITIES'
                    WHEN 'Salud' THEN 'HEALTH'
                    WHEN 'Educación' THEN 'EDUCATION'
                    WHEN 'Entretenimiento' THEN 'ENTERTAINMENT'
                    WHEN 'Compras' THEN 'SHOPPING'
                    WHEN 'Viajes' THEN 'TRAVEL'
                    WHEN 'Otros' THEN 'OTHER'
                END;
                """);

            migrationBuilder.Sql(
                """
                UPDATE "payment_methods" SET "code" = CASE "name"
                    WHEN 'Efectivo' THEN 'CASH'
                    WHEN 'Tarjeta de débito' THEN 'DEBIT_CARD'
                    WHEN 'Tarjeta de crédito' THEN 'CREDIT_CARD'
                    WHEN 'Transferencia' THEN 'BANK_TRANSFER'
                    WHEN 'Otro' THEN 'OTHER'
                END;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "payment_methods",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "categories",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_code",
                table: "payment_methods",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_code",
                table: "categories",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_payment_methods_code",
                table: "payment_methods");

            migrationBuilder.DropIndex(
                name: "IX_categories_code",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "code",
                table: "payment_methods");

            migrationBuilder.DropColumn(
                name: "code",
                table: "categories");

            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "categories",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
