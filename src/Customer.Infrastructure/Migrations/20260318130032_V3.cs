using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxEvents_OccurredAt_ProcessedAt_AggregateId",
                table: "OutboxEvents");

            migrationBuilder.AlterColumn<Guid>(
                name: "AggregateId",
                table: "OutboxEvents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedUntil",
                table: "OutboxEvents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxEvents_OccurredAt_ProcessedAt_AggregateId_LockedUntil",
                table: "OutboxEvents",
                columns: new[] { "OccurredAt", "ProcessedAt", "AggregateId", "LockedUntil" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxEvents_OccurredAt_ProcessedAt_AggregateId_LockedUntil",
                table: "OutboxEvents");

            migrationBuilder.DropColumn(
                name: "LockedUntil",
                table: "OutboxEvents");

            migrationBuilder.AlterColumn<Guid>(
                name: "AggregateId",
                table: "OutboxEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxEvents_OccurredAt_ProcessedAt_AggregateId",
                table: "OutboxEvents",
                columns: new[] { "OccurredAt", "ProcessedAt", "AggregateId" });
        }
    }
}
