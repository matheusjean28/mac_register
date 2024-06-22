using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationModelAtBuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Devices_DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_DeviceId",
                table: "Problems",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Devices_DeviceId",
                table: "Problems",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Devices_DeviceId",
                table: "Problems");

            migrationBuilder.DropIndex(
                name: "IX_Problems_DeviceId",
                table: "Problems");

            migrationBuilder.AddColumn<string>(
                name: "DeviceOwnerDeviceId",
                table: "Problems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Problems_DeviceOwnerDeviceId",
                table: "Problems",
                column: "DeviceOwnerDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Problems_Devices_DeviceOwnerDeviceId",
                table: "Problems",
                column: "DeviceOwnerDeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId");
        }
    }
}
