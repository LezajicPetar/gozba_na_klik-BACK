using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik.Migrations
{
    /// <inheritdoc />
    public partial class FixWorkTimeSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "WorkTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Start",
                table: "WorkTimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "End",
                table: "WorkTimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DayOfWeek", table: "WorkTimes");
            migrationBuilder.DropColumn(name: "Start", table: "WorkTimes");
            migrationBuilder.DropColumn(name: "End", table: "WorkTimes");
        }

    }
}
