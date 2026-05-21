using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAB251InterportCargoAssignment2Grp21.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeCredentialsNewEmployeeSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "LocalEmployeeCredentialId", "Email", "EmployeeKey" },
                values: new object[] { 1, "t.williams@company.com", "QUOTE123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "LocalEmployeeCredentialId",
                keyValue: 1);
        }
    }
}
