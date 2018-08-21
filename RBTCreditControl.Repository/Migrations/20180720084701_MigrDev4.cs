using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBTCreditControl.Repository.Migrations
{
    public partial class MigrDev4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corporate_User_OstSubmittedBy",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "OstSubmittedFlag",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "OstSubmittedOn",
                table: "Corporate");

            migrationBuilder.RenameColumn(
                name: "OstSubmittedBy",
                table: "Corporate",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Corporate_OstSubmittedBy",
                table: "Corporate",
                newName: "IX_Corporate_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "UpdateFlag",
                table: "Corporate",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddForeignKey(
                name: "FK_Corporate_User_UserId",
                table: "Corporate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Corporate_User_UserId",
                table: "Corporate");

            migrationBuilder.DropColumn(
                name: "UpdateFlag",
                table: "Corporate");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Corporate",
                newName: "OstSubmittedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Corporate_UserId",
                table: "Corporate",
                newName: "IX_Corporate_OstSubmittedBy");

            migrationBuilder.AddColumn<int>(
                name: "OstSubmittedFlag",
                table: "Corporate",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<DateTime>(
                name: "OstSubmittedOn",
                table: "Corporate",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Corporate_User_OstSubmittedBy",
                table: "Corporate",
                column: "OstSubmittedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
