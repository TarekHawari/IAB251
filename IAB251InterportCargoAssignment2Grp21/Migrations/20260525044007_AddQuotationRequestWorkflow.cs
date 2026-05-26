using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAB251InterportCargoAssignment2Grp21.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationRequestWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustomerId");

            migrationBuilder.CreateTable(
                name: "CustomerNotifications",
                columns: table => new
                {
                    CustomerNotificationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotationRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotifications", x => x.CustomerNotificationId);
                });

            migrationBuilder.CreateTable(
                name: "QuotationRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    NumberOfContainers = table.Column<int>(type: "INTEGER", nullable: false),
                    NatureOfPackage = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    ImportExportType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PackingType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    RequiresQuarantine = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OfficerMessage = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationRequests", x => x.RequestId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerNotifications");

            migrationBuilder.DropTable(
                name: "QuotationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustomerId");
        }
    }
}
