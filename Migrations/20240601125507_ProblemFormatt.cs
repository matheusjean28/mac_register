using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class ProblemFormatt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Problem",
                table: "MacstoDbs");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Devices");

            migrationBuilder.AddColumn<bool>(
                name: "RemoteAcess",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProblemTreatWrapper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: true),
                    MacToDatabaseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemTreatWrapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemTreatWrapper_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProblemTreatWrapper_MacstoDbs_MacToDatabaseId",
                        column: x => x.MacToDatabaseId,
                        principalTable: "MacstoDbs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsedAtWrapper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedAtWrapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedAtWrapper_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTreatWrapper_DeviceId",
                table: "ProblemTreatWrapper",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemTreatWrapper_MacToDatabaseId",
                table: "ProblemTreatWrapper",
                column: "MacToDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAtWrapper_DeviceId",
                table: "UsedAtWrapper",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProblemTreatWrapper");

            migrationBuilder.DropTable(
                name: "UsedAtWrapper");

            migrationBuilder.DropColumn(
                name: "RemoteAcess",
                table: "Devices");

            migrationBuilder.AddColumn<bool>(
                name: "Problem",
                table: "MacstoDbs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Devices",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
