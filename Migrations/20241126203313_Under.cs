using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Allurerealstate.Migrations
{
    /// <inheritdoc />
    public partial class Under : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UnderConstruction",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnderConstruction",
                table: "Properties");
        }
    }
}
