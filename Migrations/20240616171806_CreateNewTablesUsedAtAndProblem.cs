using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewTablesUsedAtAndProblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemTreatWrapper_Devices_DeviceId",
                table: "ProblemTreatWrapper");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtWrapper_Devices_DeviceId",
                table: "UsedAtWrapper");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedAtWrapper",
                table: "UsedAtWrapper");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemTreatWrapper",
                table: "ProblemTreatWrapper");

            migrationBuilder.RenameTable(
                name: "UsedAtWrapper",
                newName: "UsedAtClient");

            migrationBuilder.RenameTable(
                name: "ProblemTreatWrapper",
                newName: "Problems");

            migrationBuilder.RenameIndex(
                name: "IX_UsedAtWrapper_DeviceId",
                table: "UsedAtClient",
                newName: "IX_UsedAtClient_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemTreatWrapper_DeviceId",
                table: "Problems",
                newName: "IX_Problems_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedAtClient",
                table: "UsedAtClient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Problems",
                table: "Problems",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Problems_Devices_DeviceId",
                table: "Problems");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedAtClient_Devices_DeviceId",
                table: "UsedAtClient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedAtClient",
                table: "UsedAtClient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Problems",
                table: "Problems");

            migrationBuilder.RenameTable(
                name: "UsedAtClient",
                newName: "UsedAtWrapper");

            migrationBuilder.RenameTable(
                name: "Problems",
                newName: "ProblemTreatWrapper");

            migrationBuilder.RenameIndex(
                name: "IX_UsedAtClient_DeviceId",
                table: "UsedAtWrapper",
                newName: "IX_UsedAtWrapper_DeviceId");

            migrationBuilder.RenameIndex(
                name: "IX_Problems_DeviceId",
                table: "ProblemTreatWrapper",
                newName: "IX_ProblemTreatWrapper_DeviceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedAtWrapper",
                table: "UsedAtWrapper",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemTreatWrapper",
                table: "ProblemTreatWrapper",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemTreatWrapper_Devices_DeviceId",
                table: "ProblemTreatWrapper",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedAtWrapper_Devices_DeviceId",
                table: "UsedAtWrapper",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "DeviceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
