using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LifeStyleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LifestyleInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SessionsPerWeek = table.Column<int>(type: "int", nullable: true),
                    AverageDurationMinutes = table.Column<int>(type: "int", nullable: true),
                    AverageHoursOfSleep = table.Column<double>(type: "float", nullable: true),
                    SleepQualityRating = table.Column<int>(type: "int", nullable: true),
                    SpecificProblems = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StressLevel = table.Column<int>(type: "int", nullable: true),
                    IsSmoker = table.Column<bool>(type: "bit", nullable: true),
                    CigarettesPerDay = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LifestyleInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LifestyleInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LifestyleInfos_PatientId",
                table: "LifestyleInfos",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LifestyleInfos");
        }
    }
}
