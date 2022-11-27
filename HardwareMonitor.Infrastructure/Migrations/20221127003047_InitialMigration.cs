using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareMonitor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemsInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAuthorised = table.Column<bool>(type: "bit", nullable: false),
                    SystemMacs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemsInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemInfoId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemReadings_SystemsInfos_SystemInfoId",
                        column: x => x.SystemInfoId,
                        principalTable: "SystemsInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OsNameVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpuInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpuCores = table.Column<int>(type: "int", nullable: false),
                    TotalMemory = table.Column<double>(type: "float", nullable: false),
                    NetworkAdapters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemReadingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSpecs_SystemReadings_SystemReadingId",
                        column: x => x.SystemReadingId,
                        principalTable: "SystemReadings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CpuTotalUsage = table.Column<double>(type: "float", nullable: false),
                    CpuPerCoreUsage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiskUsage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemoryUsage = table.Column<double>(type: "float", nullable: false),
                    BytesReceived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BytesSent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemUptime = table.Column<double>(type: "float", nullable: false),
                    SystemReadingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usages_SystemReadings_SystemReadingId",
                        column: x => x.SystemReadingId,
                        principalTable: "SystemReadings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemReadings_SystemInfoId",
                table: "SystemReadings",
                column: "SystemInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSpecs_SystemReadingId",
                table: "SystemSpecs",
                column: "SystemReadingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usages_SystemReadingId",
                table: "Usages",
                column: "SystemReadingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSpecs");

            migrationBuilder.DropTable(
                name: "Usages");

            migrationBuilder.DropTable(
                name: "SystemReadings");

            migrationBuilder.DropTable(
                name: "SystemsInfos");
        }
    }
}
