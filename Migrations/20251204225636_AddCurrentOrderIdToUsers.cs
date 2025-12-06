using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik.Migrations
{
    public partial class AddCurrentOrderIdToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentOrderId",
                table: "Users",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentOrderId",
                table: "Users");
        }
    }
}
