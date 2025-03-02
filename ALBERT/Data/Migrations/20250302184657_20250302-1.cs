using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ALBERT.Data.Migrations
{
    /// <inheritdoc />
    public partial class _202503021 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_ChefId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ChefId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ChefId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChefId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ChefId",
                table: "Orders",
                column: "ChefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_ChefId",
                table: "Orders",
                column: "ChefId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
