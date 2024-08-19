using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComapnyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "companyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ComapnyId",
                table: "AspNetUsers",
                column: "ComapnyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_ComapnyId",
                table: "AspNetUsers",
                column: "ComapnyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_ComapnyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ComapnyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ComapnyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "companyId",
                table: "AspNetUsers");
        }
    }
}
