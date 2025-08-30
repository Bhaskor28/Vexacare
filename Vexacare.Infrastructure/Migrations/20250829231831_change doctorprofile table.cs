using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedoctorprofiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerConsultation",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SessionDuration",
                table: "DoctorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPatientsPerSlot",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotDuration",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerConsultation",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "SessionDuration",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "MaxPatientsPerSlot",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "SlotDuration",
                table: "Availabilities");
        }
    }
}
