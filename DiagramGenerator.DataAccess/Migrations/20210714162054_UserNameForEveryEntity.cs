using Microsoft.EntityFrameworkCore.Migrations;

namespace DiagramGenerator.DataAccess.Migrations
{
    public partial class UserNameForEveryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Supplier",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Requirement",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Requirement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Output",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Output",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Method",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Method",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Input",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Input",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Diagram",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Diagram",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Criterion",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Criterion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_Email",
                table: "Supplier",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_Email",
                table: "Requirement",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Output_Email",
                table: "Output",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Method_Email",
                table: "Method",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Input_Email",
                table: "Input",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Diagram_Email",
                table: "Diagram",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Criterion_Email",
                table: "Criterion",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Email",
                table: "Client",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_AspNetUsers_Email",
                table: "Client",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterion_AspNetUsers_Email",
                table: "Criterion",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagram_AspNetUsers_Email",
                table: "Diagram",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Input_AspNetUsers_Email",
                table: "Input",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Method_AspNetUsers_Email",
                table: "Method",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Output_AspNetUsers_Email",
                table: "Output",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requirement_AspNetUsers_Email",
                table: "Requirement",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_AspNetUsers_Email",
                table: "Supplier",
                column: "Email",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_AspNetUsers_Email",
                table: "Client");

            migrationBuilder.DropForeignKey(
                name: "FK_Criterion_AspNetUsers_Email",
                table: "Criterion");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagram_AspNetUsers_Email",
                table: "Diagram");

            migrationBuilder.DropForeignKey(
                name: "FK_Input_AspNetUsers_Email",
                table: "Input");

            migrationBuilder.DropForeignKey(
                name: "FK_Method_AspNetUsers_Email",
                table: "Method");

            migrationBuilder.DropForeignKey(
                name: "FK_Output_AspNetUsers_Email",
                table: "Output");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirement_AspNetUsers_Email",
                table: "Requirement");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_AspNetUsers_Email",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_Email",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Requirement_Email",
                table: "Requirement");

            migrationBuilder.DropIndex(
                name: "IX_Output_Email",
                table: "Output");

            migrationBuilder.DropIndex(
                name: "IX_Method_Email",
                table: "Method");

            migrationBuilder.DropIndex(
                name: "IX_Input_Email",
                table: "Input");

            migrationBuilder.DropIndex(
                name: "IX_Diagram_Email",
                table: "Diagram");

            migrationBuilder.DropIndex(
                name: "IX_Criterion_Email",
                table: "Criterion");

            migrationBuilder.DropIndex(
                name: "IX_Client_Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Requirement");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Requirement");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Output");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Output");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Method");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Method");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Input");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Input");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Diagram");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Diagram");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Criterion");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Criterion");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Client");
        }
    }
}
