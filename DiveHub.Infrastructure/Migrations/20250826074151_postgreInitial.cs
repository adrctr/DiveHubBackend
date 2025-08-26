using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiveHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class postgreInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipmentName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Dives",
                columns: table => new
                {
                    DiveId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DiveName = table.Column<string>(type: "text", nullable: false),
                    DiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Depth = table.Column<float>(type: "real", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dives", x => x.DiveId);
                    table.ForeignKey(
                        name: "FK_Dives_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiveEquipment",
                columns: table => new
                {
                    DivesDiveId = table.Column<int>(type: "integer", nullable: false),
                    EquipmentsEquipmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiveEquipment", x => new { x.DivesDiveId, x.EquipmentsEquipmentId });
                    table.ForeignKey(
                        name: "FK_DiveEquipment_Dives_DivesDiveId",
                        column: x => x.DivesDiveId,
                        principalTable: "Dives",
                        principalColumn: "DiveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiveEquipment_Equipments_EquipmentsEquipmentId",
                        column: x => x.EquipmentsEquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiveEquipment_EquipmentsEquipmentId",
                table: "DiveEquipment",
                column: "EquipmentsEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Dives_UserId",
                table: "Dives",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiveEquipment");

            migrationBuilder.DropTable(
                name: "Dives");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
