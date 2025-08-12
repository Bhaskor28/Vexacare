using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateSymptomsInfoandDietProfileInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietProfileInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DietFood = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DietTypeOther = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Vegetables = table.Column<int>(type: "int", nullable: false),
                    Fruits = table.Column<int>(type: "int", nullable: false),
                    WholeGrains = table.Column<int>(type: "int", nullable: false),
                    AnimalProteins = table.Column<int>(type: "int", nullable: false),
                    PlantProteins = table.Column<int>(type: "int", nullable: false),
                    DairyProducts = table.Column<int>(type: "int", nullable: false),
                    FermentedFoods = table.Column<int>(type: "int", nullable: false),
                    Water = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Alcohol = table.Column<int>(type: "int", nullable: true),
                    BreakfastTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LunchTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SnacksTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DinnerTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietProfileInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietProfileInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomsInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrequencyOfEvaluations = table.Column<int>(type: "int", nullable: true),
                    BristolScale = table.Column<int>(type: "int", nullable: true),
                    BloatingSeverity = table.Column<int>(type: "int", nullable: true),
                    IntestinalGas = table.Column<int>(type: "int", nullable: true),
                    AbdominalPain = table.Column<int>(type: "int", nullable: true),
                    DigestiveDifficulties = table.Column<int>(type: "int", nullable: true),
                    DiagnosedIntolerances = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CertifiedAllergies = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TestsPerformed = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomsInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomsInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietProfileInfos_PatientId",
                table: "DietProfileInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomsInfos_PatientId",
                table: "SymptomsInfos",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietProfileInfos");

            migrationBuilder.DropTable(
                name: "SymptomsInfos");
        }
    }
}
