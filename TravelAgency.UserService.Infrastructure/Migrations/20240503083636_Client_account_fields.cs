using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAgency.UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Client_account_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ClientAccount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ClientAccount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ClientAccount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ClientAccount");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ClientAccount");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ClientAccount");
        }
    }
}
