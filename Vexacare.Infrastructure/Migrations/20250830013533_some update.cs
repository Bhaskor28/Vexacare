using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class someupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "DoctorProfiles");

            migrationBuilder.DropColumn(
                name: "MaxPatientsPerSlot",
                table: "Availabilities");

            migrationBuilder.AlterColumn<int>(
                name: "SessionDuration",
                table: "DoctorProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerConsultation",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "DurationUnit",
                table: "DoctorProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SlotDuration",
                table: "Availabilities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationUnit",
                table: "DoctorProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "SessionDuration",
                table: "DoctorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerConsultation",
                table: "DoctorProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "DoctorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SlotDuration",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxPatientsPerSlot",
                table: "Availabilities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
