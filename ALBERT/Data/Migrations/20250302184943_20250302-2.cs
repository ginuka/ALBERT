using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ALBERT.Data.Migrations
{
    /// <inheritdoc />
    public partial class _202503022 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Employees_WaiterId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_WaiterId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "WaiterId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WaiterId",
                table: "Tables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Employees",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_WaiterId",
                table: "Tables",
                column: "WaiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Employees_WaiterId",
                table: "Tables",
                column: "WaiterId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
