using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class RefactDeviceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Devices");

            migrationBuilder.AddColumn<bool>(
                name: "Problem",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RemoteAcess",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Problem",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "RemoteAcess",
                table: "Devices");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Devices",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
