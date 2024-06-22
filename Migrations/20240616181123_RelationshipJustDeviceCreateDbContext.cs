using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipJustDeviceCreateDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Devices_DeviceId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_UsedAtClient_DeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_Problems_DeviceId",
                table: "Problems");

            migrationBuilder.AddColumn<string>(
                name: "DeviceOwnerDeviceId",
                table: "UsedAtClient",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceOwnerDeviceId",
                table: "Problems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsedAtClient_DeviceOwnerDeviceId",
                table: "UsedAtClient",
                column: "DeviceOwnerDeviceId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceOwnerDeviceId",
                table: "UsedAtClient",
                column: "DeviceOwnerDeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Devices_DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_UsedAtClient_DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropIndex(
                name: "IX_Problems_DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "DeviceOwnerDeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropColumn(
                name: "DeviceOwnerDeviceId",
                table: "Problems");

            migrationBuilder.CreateIndex(
                name: "IX_UsedAtClient_DeviceId",
                table: "UsedAtClient",
                column: "DeviceId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceId",
                table: "UsedAtClient",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
