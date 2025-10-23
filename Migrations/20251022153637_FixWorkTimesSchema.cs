using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik.Migrations
{
    /// <inheritdoc />
    public partial class FixWorkTimesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""WorkTimes""
                ADD COLUMN IF NOT EXISTS ""Start"" time without time zone NOT NULL DEFAULT TIME '00:00:00';");
            migrationBuilder.Sql(@"ALTER TABLE ""WorkTimes""
                ADD COLUMN IF NOT EXISTS ""End"" time without time zone NOT NULL DEFAULT TIME '00:00:00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkTimes_DayOfWeek_Range",
                table: "WorkTimes");

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkTime_DayOfWeek",
                table: "WorkTimes",
                sql: "[DayOfWeek] >= 0 AND [DayOfWeek] <= 6");
        }
    }
}
