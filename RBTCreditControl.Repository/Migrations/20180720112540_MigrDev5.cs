using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBTCreditControl.Repository.Migrations
{
    public partial class MigrDev5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OstSubmittedBy",
                table: "Corporate",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OstSubmittedOn",
                table: "Corporate",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OstSubmittedBy",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "OstSubmittedOn",
                table: "Corporate");
        }
    }
}
