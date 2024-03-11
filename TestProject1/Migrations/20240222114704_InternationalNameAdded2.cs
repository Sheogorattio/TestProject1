using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProject1.Migrations
{
    /// <inheritdoc />
    public partial class InternationalNameAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                column: "InternationalName",
                value: "ItDepartment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                column: "InternationalName",
                value: "Test");
        }
    }
}
