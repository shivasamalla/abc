using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBTCreditControl.Repository.Migrations
{
    public partial class MigrDev3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "fromDate",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "toDate",
                table: "Corporate");

            migrationBuilder.AddColumn<decimal>(
                name: "SbmtAmount",
                table: "UserCorporateAction",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SbmtFromDate",
                table: "UserCorporateAction",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SbmtToDate",
                table: "UserCorporateAction",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SbmtAmount",
                table: "UserCorporateAction");

            migrationBuilder.DropColumn(
                name: "SbmtFromDate",
                table: "UserCorporateAction");

            migrationBuilder.DropColumn(
                name: "SbmtToDate",
                table: "UserCorporateAction");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Corporate",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fromDate",
                table: "Corporate",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "toDate",
                table: "Corporate",
                nullable: true);
        }
    }
}
