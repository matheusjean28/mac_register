using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations.MainDatabaseMigrations
{
    /// <inheritdoc />
    public partial class ProblemFormatt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Problem",
                table: "DevicesToMain");

            migrationBuilder.CreateTable(
                name: "ProblemTreatWrapper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MacToDatabaseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTreatWrapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemTreatWrapper_DevicesToMain_MacToDatabaseId",
                        column: x => x.MacToDatabaseId,
                        principalTable: "DevicesToMain",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTreatWrapper_MacToDatabaseId",
                table: "ProblemTreatWrapper",
                column: "MacToDatabaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemTreatWrapper");

            migrationBuilder.AddColumn<bool>(
                name: "Problem",
                table: "DevicesToMain",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
