using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TherapiesInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TherapiesInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsedAntibioticsRecently = table.Column<bool>(type: "bit", nullable: true),
                    AntibioticName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndOfTherapyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsesProbiotics = table.Column<bool>(type: "bit", nullable: false),
                    UsesPrebiotics = table.Column<bool>(type: "bit", nullable: false),
                    UsesMinerals = table.Column<bool>(type: "bit", nullable: false),
                    UsesVitamins = table.Column<bool>(type: "bit", nullable: false),
                    UsesOtherSupplements = table.Column<bool>(type: "bit", nullable: false),
                    OtherSupplementsDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryObjective = table.Column<int>(type: "int", nullable: true),
                    SecondaryObjectives = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TherapiesInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TherapiesInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TherapiesInfos_PatientId",
                table: "TherapiesInfos",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TherapiesInfos");
        }
    }
}
