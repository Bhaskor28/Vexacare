using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasicInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "HealthInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BMI = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MainDiagnoses = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiagnosisDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DrugName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthInfos_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BasicInfos_PatientId",
                table: "BasicInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DietProfileInfos_PatientId",
                table: "DietProfileInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_GastrointestinalInfos_PatientId",
                table: "GastrointestinalInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthInfos_PatientId",
                table: "HealthInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LifestyleInfos_PatientId",
                table: "LifestyleInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomsInfos_PatientId",
                table: "SymptomsInfos",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_TherapiesInfos_PatientId",
                table: "TherapiesInfos",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BasicInfos");

            migrationBuilder.DropTable(
                name: "DietProfileInfos");

            migrationBuilder.DropTable(
                name: "GastrointestinalInfos");

            migrationBuilder.DropTable(
                name: "HealthInfos");

            migrationBuilder.DropTable(
                name: "LifestyleInfos");

            migrationBuilder.DropTable(
                name: "SymptomsInfos");

            migrationBuilder.DropTable(
                name: "TherapiesInfos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
