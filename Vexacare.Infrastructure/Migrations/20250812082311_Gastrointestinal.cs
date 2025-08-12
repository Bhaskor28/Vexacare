using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Gastrointestinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GastrointestinalInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousGIProblems = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OnsetDateOfFirstSymptoms = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TreatmentsPerformed = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GIPathology = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OtherRelevantMedicalConditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DegreeOfRelationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeOfSurgery = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfSurgery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Outcome = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastrointestinalInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GastrointestinalInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GastrointestinalInfos_PatientId",
                table: "GastrointestinalInfos",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GastrointestinalInfos");
        }
    }
}
