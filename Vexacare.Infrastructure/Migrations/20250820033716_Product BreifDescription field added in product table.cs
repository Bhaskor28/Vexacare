using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vexacare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductBreifDescriptionfieldaddedinproducttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BreifDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreifDescription",
                table: "Products");
        }
    }
}
