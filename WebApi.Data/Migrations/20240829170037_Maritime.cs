using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Maritime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terminals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PortId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terminals_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ports",
                columns: new[] { "Id", "AddedDate", "Code", "LastEditedDate", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4440), "ABCDE", new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4441), "Port A" },
                    { 2, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4443), "FGHIJ", new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4443), "Port B" }
                });

            migrationBuilder.InsertData(
                table: "Terminals",
                columns: new[] { "Id", "AddedDate", "IsActive", "LastEditedDate", "Latitude", "Longitude", "Name", "PortId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4557), true, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4558), 40.712800000000001, -74.006, "Terminal 1", 1 },
                    { 2, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4560), true, new DateTime(2024, 8, 29, 17, 0, 36, 967, DateTimeKind.Utc).AddTicks(4561), 34.052199999999999, -118.2437, "Terminal 2", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ports_Code",
                table: "Ports",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_Name_PortId",
                table: "Terminals",
                columns: new[] { "Name", "PortId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_PortId",
                table: "Terminals",
                column: "PortId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Terminals");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Ports");
        }
    }
}
