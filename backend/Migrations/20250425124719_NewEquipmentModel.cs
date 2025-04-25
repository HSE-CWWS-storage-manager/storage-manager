using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class NewEquipmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Equipments");

            migrationBuilder.AddColumn<Guid>(
                name: "FromId",
                table: "EquipmentWriteOffs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FromId",
                table: "EquipmentTransfers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "InventoryNumber",
                table: "Equipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentWriteOffs_FromId",
                table: "EquipmentWriteOffs",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTransfers_FromId",
                table: "EquipmentTransfers",
                column: "FromId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentTransfers_Warehouse_FromId",
                table: "EquipmentTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentWriteOffs_Warehouse_FromId",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentWriteOffs_FromId",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentTransfers_FromId",
                table: "EquipmentTransfers");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "EquipmentTransfers");

            migrationBuilder.DropColumn(
                name: "InventoryNumber",
                table: "Equipments");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Equipments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
