using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class fixTableNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_filesUpload",
                table: "filesUpload");

            migrationBuilder.RenameTable(
                name: "filesUpload",
                newName: "FilesUploads");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilesUploads",
                table: "FilesUploads",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilesUploads",
                table: "FilesUploads");

            migrationBuilder.RenameTable(
                name: "FilesUploads",
                newName: "filesUpload");

            migrationBuilder.AddPrimaryKey(
                name: "PK_filesUpload",
                table: "filesUpload",
                column: "Id");
        }
    }
}
