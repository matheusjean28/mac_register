using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class CategoryCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Makers",
                columns: table => new
                {
                    MakerId = table.Column<string>(type: "TEXT", nullable: false),
                    MakerName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makers", x => x.MakerId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCategories",
                columns: table => new
                {
                    DeviceCategoryId = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceCategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    OperationMode = table.Column<int>(type: "INTEGER", nullable: false),
                    MakerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCategories", x => x.DeviceCategoryId);
                    table.ForeignKey(
                        name: "FK_DeviceCategories_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "MakerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCategories_MakerId",
                table: "DeviceCategories",
                column: "MakerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceCategories");

            migrationBuilder.DropTable(
                name: "Makers");
        }
    }
}
