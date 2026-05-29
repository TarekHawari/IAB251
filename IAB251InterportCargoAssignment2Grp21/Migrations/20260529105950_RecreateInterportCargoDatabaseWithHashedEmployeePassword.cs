using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAB251InterportCargoAssignment2Grp21.Migrations
{
    /// <inheritdoc />
    public partial class RecreateInterportCargoDatabaseWithHashedEmployeePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "LocalEmployeeCredentialId",
                keyValue: 1,
                column: "EmployeeKey",
                value: "$2a$11$6CnzNEQoVI3kEIvjTFQ3suP3Ynf2WrqLFPLnheBXWqhNAER3eypj2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "LocalEmployeeCredentialId",
                keyValue: 1,
                column: "EmployeeKey",
                value: "QUOTE123");
        }
    }
}
