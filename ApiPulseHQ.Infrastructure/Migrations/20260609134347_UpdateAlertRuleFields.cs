using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPulseHQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlertRuleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AlertOnFailure",
                table: "AlertRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AlertOnRecovery",
                table: "AlertRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CooldownMinutes",
                table: "AlertRules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAlertSentAt",
                table: "AlertRules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "AlertRules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertOnFailure",
                table: "AlertRules");

            migrationBuilder.DropColumn(
                name: "AlertOnRecovery",
                table: "AlertRules");

            migrationBuilder.DropColumn(
                name: "CooldownMinutes",
                table: "AlertRules");

            migrationBuilder.DropColumn(
                name: "LastAlertSentAt",
                table: "AlertRules");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AlertRules");
        }
    }
}
