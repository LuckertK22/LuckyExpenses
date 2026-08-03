using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuckyExpenses.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_expenses_category_id",
                table: "expenses",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_payment_method_id",
                table: "expenses",
                column: "payment_method_id");

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_categories_category_id",
                table: "expenses",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_payment_methods_payment_method_id",
                table: "expenses",
                column: "payment_method_id",
                principalTable: "payment_methods",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_users_user_id",
                table: "expenses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_expenses_categories_category_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_payment_methods_payment_method_id",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_users_user_id",
                table: "expenses");

            migrationBuilder.DropIndex(
                name: "IX_expenses_category_id",
                table: "expenses");

            migrationBuilder.DropIndex(
                name: "IX_expenses_payment_method_id",
                table: "expenses");
        }
    }
}
