using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class changeDeviceCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceCategoryName",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceCategoryName",
                table: "Devices",
                newName: "DeviceCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DeviceCategoryName",
                table: "Devices",
                newName: "IX_Devices_DeviceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceCategoryId",
                table: "Devices",
                column: "DeviceCategoryId",
                principalTable: "DeviceCategories",
                principalColumn: "DeviceCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceCategoryId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceCategoryId",
                table: "Devices",
                newName: "DeviceCategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DeviceCategoryId",
                table: "Devices",
                newName: "IX_Devices_DeviceCategoryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceCategoryName",
                table: "Devices",
                column: "DeviceCategoryName",
                principalTable: "DeviceCategories",
                principalColumn: "DeviceCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
