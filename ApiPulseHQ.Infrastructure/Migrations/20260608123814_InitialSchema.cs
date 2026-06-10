using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPulseHQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceEndpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CheckIntervalSeconds = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEndpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEndpoints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatusPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusPages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AlertRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceEndpointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThresholdSeconds = table.Column<int>(type: "int", nullable: false),
                    NotificationEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertRules_ServiceEndpoints_ServiceEndpointId",
                        column: x => x.ServiceEndpointId,
                        principalTable: "ServiceEndpoints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceCheckLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceEndpointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    ResponseTimeMs = table.Column<long>(type: "bigint", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CheckedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCheckLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCheckLogs_ServiceEndpoints_ServiceEndpointId",
                        column: x => x.ServiceEndpointId,
                        principalTable: "ServiceEndpoints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatusPageServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceEndpointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusPageServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusPageServices_ServiceEndpoints_ServiceEndpointId",
                        column: x => x.ServiceEndpointId,
                        principalTable: "ServiceEndpoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StatusPageServices_StatusPages_StatusPageId",
                        column: x => x.StatusPageId,
                        principalTable: "StatusPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertRules_ServiceEndpointId",
                table: "AlertRules",
                column: "ServiceEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCheckLogs_ServiceEndpointId",
                table: "ServiceCheckLogs",
                column: "ServiceEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEndpoints_UserId",
                table: "ServiceEndpoints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPages_UserId",
                table: "StatusPages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPageServices_ServiceEndpointId",
                table: "StatusPageServices",
                column: "ServiceEndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusPageServices_StatusPageId",
                table: "StatusPageServices",
                column: "StatusPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertRules");

            migrationBuilder.DropTable(
                name: "ServiceCheckLogs");

            migrationBuilder.DropTable(
                name: "StatusPageServices");

            migrationBuilder.DropTable(
                name: "ServiceEndpoints");

            migrationBuilder.DropTable(
                name: "StatusPages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
