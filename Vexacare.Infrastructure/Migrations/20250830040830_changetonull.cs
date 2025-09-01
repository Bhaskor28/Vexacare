using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetonull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorProfiles_AvailableDays_AvailableDaysId",
                table: "DoctorProfiles");

            migrationBuilder.DropTable(
                name: "AvailableWeekofDay");

            migrationBuilder.DropColumn(
                name: "DoctorProfileId",
                table: "WeekofDays");

            migrationBuilder.DropColumn(
                name: "WeekDayId",
                table: "Available");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableDaysId",
                table: "DoctorProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorProfiles_AvailableDays_AvailableDaysId",
                table: "DoctorProfiles",
                column: "AvailableDaysId",
                principalTable: "AvailableDays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorProfiles_AvailableDays_AvailableDaysId",
                table: "DoctorProfiles");

            migrationBuilder.AddColumn<int>(
                name: "DoctorProfileId",
                table: "WeekofDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AvailableDaysId",
                table: "DoctorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekDayId",
                table: "Available",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AvailableWeekofDay",
                columns: table => new
                {
                    AvailablesId = table.Column<int>(type: "int", nullable: false),
                    WeekofDaysId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableWeekofDay", x => new { x.AvailablesId, x.WeekofDaysId });
                    table.ForeignKey(
                        name: "FK_AvailableWeekofDay_Available_AvailablesId",
                        column: x => x.AvailablesId,
                        principalTable: "Available",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableWeekofDay_WeekofDays_WeekofDaysId",
                        column: x => x.WeekofDaysId,
                        principalTable: "WeekofDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableWeekofDay_WeekofDaysId",
                table: "AvailableWeekofDay",
                column: "WeekofDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorProfiles_AvailableDays_AvailableDaysId",
                table: "DoctorProfiles",
                column: "AvailableDaysId",
                principalTable: "AvailableDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
