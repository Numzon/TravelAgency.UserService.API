using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAgency.UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientAccountMigrationv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientAccountId",
                table: "CreditCard");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientAccountId",
                table: "CreditCard",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
