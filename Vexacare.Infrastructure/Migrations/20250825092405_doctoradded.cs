using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class doctoradded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorProfiles_Availabilities_AvailabilityId",
                table: "DoctorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_DoctorProfiles_AvailabilityId",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Availabilities");

            migrationBuilder.AlterColumn<decimal>(
                name: "PatientsCount",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorProfileId",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Availabilities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Availabilities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities",
                column: "DoctorProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_DoctorProfiles_DoctorProfileId",
                table: "Availabilities",
                column: "DoctorProfileId",
                principalTable: "DoctorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_DoctorProfiles_DoctorProfileId",
                table: "Availabilities");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "DoctorProfileId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Availabilities");

            migrationBuilder.AlterColumn<int>(
                name: "PatientsCount",
                table: "DoctorProfiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Availabilities",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_DoctorProfiles_AvailabilityId",
                table: "DoctorProfiles",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorProfiles_Availabilities_AvailabilityId",
                table: "DoctorProfiles",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
