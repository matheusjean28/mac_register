﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MacSave.Migrations
{
    /// <inheritdoc />
    public partial class MacToDatabaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacstoDbs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Mac = table.Column<string>(type: "TEXT", nullable: false),
                    Problem = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemoteAccess = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacstoDbs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacstoDbs");
        }
    }
}
