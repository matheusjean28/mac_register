using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceCategoryAndImproveGetAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceName",
                table: "Devices",
                column: "DeviceName");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceName",
                table: "Devices",
                column: "DeviceName",
                principalTable: "DeviceCategories",
                principalColumn: "DeviceCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceCategories_DeviceName",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceName",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "Devices");
        }
    }
}
