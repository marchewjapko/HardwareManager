using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemMonitor.Infrastructure.Migrations
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
                    MemoryUsage = table.Column<double>(type: "float", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DisksSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiskSize = table.Column<double>(type: "float", nullable: false),
                    SystemSpecsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisksSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisksSpecs_SystemSpecs_SystemSpecsId",
                        column: x => x.SystemSpecsId,
                        principalTable: "SystemSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetworksSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdapterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bandwidth = table.Column<double>(type: "float", nullable: false),
                    SystemSpecsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworksSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworksSpecs_SystemSpecs_SystemSpecsId",
                        column: x => x.SystemSpecsId,
                        principalTable: "SystemSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuPerCoreUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Instance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Usage = table.Column<double>(type: "float", nullable: false),
                    SystemUsageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuPerCoreUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuPerCoreUsages_Usages_SystemUsageId",
                        column: x => x.SystemUsageId,
                        principalTable: "Usages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiskUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Usage = table.Column<double>(type: "float", nullable: false),
                    SystemUsageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiskUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiskUsages_Usages_SystemUsageId",
                        column: x => x.SystemUsageId,
                        principalTable: "Usages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetworkUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdapterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BytesSent = table.Column<double>(type: "float", nullable: false),
                    BytesReceived = table.Column<double>(type: "float", nullable: false),
                    SystemUsageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetworkUsages_Usages_SystemUsageId",
                        column: x => x.SystemUsageId,
                        principalTable: "Usages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuPerCoreUsages_SystemUsageId",
                table: "CpuPerCoreUsages",
                column: "SystemUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_DisksSpecs_SystemSpecsId",
                table: "DisksSpecs",
                column: "SystemSpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiskUsages_SystemUsageId",
                table: "DiskUsages",
                column: "SystemUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworksSpecs_SystemSpecsId",
                table: "NetworksSpecs",
                column: "SystemSpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkUsages_SystemUsageId",
                table: "NetworkUsages",
                column: "SystemUsageId");

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
                name: "CpuPerCoreUsages");

            migrationBuilder.DropTable(
                name: "DisksSpecs");

            migrationBuilder.DropTable(
                name: "DiskUsages");

            migrationBuilder.DropTable(
                name: "NetworksSpecs");

            migrationBuilder.DropTable(
                name: "NetworkUsages");

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
