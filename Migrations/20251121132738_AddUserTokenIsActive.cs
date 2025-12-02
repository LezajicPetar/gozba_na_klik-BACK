using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gozba_na_klik.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTokenIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Open",
                table: "RestaurantWorkTimes",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Close",
                table: "RestaurantWorkTimes",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Open",
                table: "RestaurantWorkTimes",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Close",
                table: "RestaurantWorkTimes",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "ProfilePicture", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin1@gozba.com", "Admin", "One", "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n", null, "Admin", "Admin1" },
                    { 2, "admin2@gozba.com", "Admin", "Two", "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n", null, "Admin", "Admin2" },
                    { 3, "admin3@gozba.com", "Admin", "Three", "$2a$11$VdTkF.NE1aw8uZmfFO51OuxlW9qrvbx7W8g3iKw6aHcuC1vHfMJt6\r\n", null, "Admin", "Admin3" }
                });
        }
    }
}
