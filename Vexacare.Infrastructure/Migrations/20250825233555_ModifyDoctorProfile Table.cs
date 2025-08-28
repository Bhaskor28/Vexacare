using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDoctorProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "ConsultationType",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "PatientsCount",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "DoctorProfiles");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "DoctorProfiles",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Languages",
                table: "DoctorProfiles",
                newName: "ProfileImagePath");

            migrationBuilder.RenameColumn(
                name: "FeePeriod",
                table: "DoctorProfiles",
                newName: "EducationDetails");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "DoctorProfiles",
                newName: "MyProperty");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DoctorProfiles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "DoctorProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "DoctorProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorProfiles_UserId",
                table: "DoctorProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities",
                column: "DoctorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorProfiles_AspNetUsers_UserId",
                table: "DoctorProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorProfiles_AspNetUsers_UserId",
                table: "DoctorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_DoctorProfiles_UserId",
                table: "DoctorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "DoctorProfiles");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DoctorProfiles",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "DoctorProfiles",
                newName: "Languages");

            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "DoctorProfiles",
                newName: "AvailabilityId");

            migrationBuilder.RenameColumn(
                name: "EducationDetails",
                table: "DoctorProfiles",
                newName: "FeePeriod");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ConsultationType",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PatientsCount",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_DoctorProfileId",
                table: "Availabilities",
                column: "DoctorProfileId",
                unique: true);
        }
    }
}
