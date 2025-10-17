using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAllergens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserAllergens",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
