using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_UserId",
                table: "WorkTimes");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "WorkTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "End",
                table: "WorkTimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Start",
                table: "WorkTimes",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$12$97Po1ExL9B3PTNSyDYBlmetfcdxQNuLWdRQ06l.A8eC0pJ9s6Zee2");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_UserId_DayOfWeek",
                table: "WorkTimes",
                columns: new[] { "UserId", "DayOfWeek" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_WorkTime_DayOfWeek",
                table: "WorkTimes",
                sql: "[DayOfWeek] >= 0 AND [DayOfWeek] <= 6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkTimes_UserId_DayOfWeek",
                table: "WorkTimes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_WorkTime_DayOfWeek",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "End",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "WorkTimes");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimes_UserId",
                table: "WorkTimes",
                column: "UserId");
        }
    }
}
