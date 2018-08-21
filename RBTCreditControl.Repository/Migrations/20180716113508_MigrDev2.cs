using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBTCreditControl.Repository.Migrations
{
    public partial class MigrDev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_actions_Corporate_FK_CorporateId",
                table: "actions");

            migrationBuilder.DropForeignKey(
                name: "FK_actions_StatusMaster_FK_CorporateStatusMasterId",
                table: "actions");

            migrationBuilder.DropForeignKey(
                name: "FK_actions_User_UpdatedBy",
                table: "actions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_actions",
                table: "actions");

            migrationBuilder.RenameTable(
                name: "actions",
                newName: "UserCorporateAction");

            migrationBuilder.RenameIndex(
                name: "IX_actions_UpdatedBy",
                table: "UserCorporateAction",
                newName: "IX_UserCorporateAction_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_actions_FK_CorporateStatusMasterId",
                table: "UserCorporateAction",
                newName: "IX_UserCorporateAction_FK_CorporateStatusMasterId");

            migrationBuilder.RenameIndex(
                name: "IX_actions_FK_CorporateId",
                table: "UserCorporateAction",
                newName: "IX_UserCorporateAction_FK_CorporateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCorporateAction",
                table: "UserCorporateAction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCorporateAction_Corporate_FK_CorporateId",
                table: "UserCorporateAction",
                column: "FK_CorporateId",
                principalTable: "Corporate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCorporateAction_StatusMaster_FK_CorporateStatusMasterId",
                table: "UserCorporateAction",
                column: "FK_CorporateStatusMasterId",
                principalTable: "StatusMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCorporateAction_User_UpdatedBy",
                table: "UserCorporateAction",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCorporateAction_Corporate_FK_CorporateId",
                table: "UserCorporateAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCorporateAction_StatusMaster_FK_CorporateStatusMasterId",
                table: "UserCorporateAction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCorporateAction_User_UpdatedBy",
                table: "UserCorporateAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCorporateAction",
                table: "UserCorporateAction");

            migrationBuilder.RenameTable(
                name: "UserCorporateAction",
                newName: "actions");

            migrationBuilder.RenameIndex(
                name: "IX_UserCorporateAction_UpdatedBy",
                table: "actions",
                newName: "IX_actions_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserCorporateAction_FK_CorporateStatusMasterId",
                table: "actions",
                newName: "IX_actions_FK_CorporateStatusMasterId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCorporateAction_FK_CorporateId",
                table: "actions",
                newName: "IX_actions_FK_CorporateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_actions",
                table: "actions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_actions_Corporate_FK_CorporateId",
                table: "actions",
                column: "FK_CorporateId",
                principalTable: "Corporate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_actions_StatusMaster_FK_CorporateStatusMasterId",
                table: "actions",
                column: "FK_CorporateStatusMasterId",
                principalTable: "StatusMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_actions_User_UpdatedBy",
                table: "actions",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
