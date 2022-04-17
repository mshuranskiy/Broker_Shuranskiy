using Microsoft.EntityFrameworkCore.Migrations;

namespace Broker_Shuranskiy.Migrations
{
    public partial class ChangeUsersandBags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Users_UsersId",
                table: "Bags");

            migrationBuilder.DropIndex(
                name: "IX_Bags_UsersId",
                table: "Bags");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Bags");

            migrationBuilder.AddColumn<int>(
                name: "Id_User",
                table: "Bags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_User",
                table: "Bags");

            migrationBuilder.AddColumn<long>(
                name: "UsersId",
                table: "Bags",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bags_UsersId",
                table: "Bags",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Users_UsersId",
                table: "Bags",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
