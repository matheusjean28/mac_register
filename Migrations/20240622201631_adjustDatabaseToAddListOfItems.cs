using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class adjustDatabaseToAddListOfItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_UsedAtClient_DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropColumn(
                name: "DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAtClient_DeviceId",
                table: "UsedAtClient",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceId",
                table: "UsedAtClient",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_UsedAtClient_DeviceId",
                table: "UsedAtClient");

            migrationBuilder.AddColumn<string>(
                name: "DeviceOwnerDeviceId",
                table: "UsedAtClient",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsedAtClient_DeviceOwnerDeviceId",
                table: "UsedAtClient",
                column: "DeviceOwnerDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceOwnerDeviceId",
                table: "UsedAtClient",
                column: "DeviceOwnerDeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId");
        }
    }
}
