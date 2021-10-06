using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagramGenerator.DataAccess.Migrations
{
    public partial class DiagramSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputSupplier");

            migrationBuilder.CreateTable(
                name: "DiagramSupplier",
                columns: table => new
                {
                    DiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiagramSupplier", x => new { x.SupplierId, x.DiagramId });
                    table.ForeignKey(
                        name: "FK_DiagramSupplier_Diagram_DiagramId",
                        column: x => x.DiagramId,
                        principalTable: "Diagram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiagramSupplier_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiagramSupplier_DiagramId",
                table: "DiagramSupplier",
                column: "DiagramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiagramSupplier");

            migrationBuilder.CreateTable(
                name: "InputSupplier",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InputId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputSupplier", x => new { x.SupplierId, x.InputId });
                    table.ForeignKey(
                        name: "FK_InputSupplier_Input_InputId",
                        column: x => x.InputId,
                        principalTable: "Input",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InputSupplier_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputSupplier_InputId",
                table: "InputSupplier",
                column: "InputId");
        }
    }
}
