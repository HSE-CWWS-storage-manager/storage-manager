using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehousesAndEquipmentRemains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTransfers_Warehouse_FromId",
                table: "EquipmentTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentWriteOffs_Warehouse_FromId",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "Warehouses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EquipmentRemains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    OnStock = table.Column<int>(type: "integer", nullable: false),
                    OnLoan = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentRemains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentRemains_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentRemains_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRemains_EquipmentId",
                table: "EquipmentRemains",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRemains_WarehouseId",
                table: "EquipmentRemains",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTransfers_Warehouses_FromId",
                table: "EquipmentTransfers",
                column: "FromId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentWriteOffs_Warehouses_FromId",
                table: "EquipmentWriteOffs",
                column: "FromId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTransfers_Warehouses_FromId",
                table: "EquipmentTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentWriteOffs_Warehouses_FromId",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropTable(
                name: "EquipmentRemains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "Warehouse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentTransfers_Warehouse_FromId",
                table: "EquipmentTransfers",
                column: "FromId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentWriteOffs_Warehouse_FromId",
                table: "EquipmentWriteOffs",
                column: "FromId",
                principalTable: "Warehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
