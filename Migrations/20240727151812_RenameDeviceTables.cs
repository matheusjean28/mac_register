using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class RenameDeviceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Problems",
                newName: "ProblemName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Problems",
                newName: "ProblemDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProblemName",
                table: "Problems",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProblemDescription",
                table: "Problems",
                newName: "Description");
        }
    }
}
