using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProject1.Migrations
{
    /// <inheritdoc />
    public partial class EfContextEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("d3c376e4-bce3-4d85-aba4-e3cf49612c94"),
                column: "InternationalName",
                value: "IT Department");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ManagerId",
                table: "Sales",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdChief",
                table: "Managers",
                column: "IdChief");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdMainDep",
                table: "Managers",
                column: "IdMainDep");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdSecDep",
                table: "Managers",
                column: "IdSecDep");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Departments_IdMainDep",
                table: "Managers",
                column: "IdMainDep",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Departments_IdSecDep",
                table: "Managers",
                column: "IdSecDep",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Managers_IdChief",
                table: "Managers",
                column: "IdChief",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Managers_ManagerId",
                table: "Sales",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Departments_IdMainDep",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Departments_IdSecDep",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Managers_IdChief",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Managers_ManagerId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ManagerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ProductId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdChief",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdMainDep",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdSecDep",
                table: "Managers");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                column: "InternationalName",
                value: "IT Department");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("d3c376e4-bce3-4d85-aba4-e3cf49612c94"),
                column: "InternationalName",
                value: null);
        }
    }
}
