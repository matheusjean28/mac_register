using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class deviceCategoryNameCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceName",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceName",
                table: "Devices",
                newName: "DeviceCategoryName");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DeviceName",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceCategoryName",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceCategoryName",
                table: "Devices",
                newName: "DeviceName");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_DeviceCategoryName",
                table: "Devices",
                newName: "IX_Devices_DeviceName");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceName",
                table: "Devices",
                column: "DeviceName",
                principalTable: "DeviceCategories",
                principalColumn: "DeviceCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
