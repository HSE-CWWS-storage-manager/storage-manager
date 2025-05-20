using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MoreIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_Name_Group",
                table: "Students",
                columns: new[] { "Name", "Group" });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentWriteOffs_Date",
                table: "EquipmentWriteOffs",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTransfers_IssueDate",
                table: "EquipmentTransfers",
                column: "IssueDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Name_Group",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentWriteOffs_Date",
                table: "EquipmentWriteOffs");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentTransfers_IssueDate",
                table: "EquipmentTransfers");
        }
    }
}
