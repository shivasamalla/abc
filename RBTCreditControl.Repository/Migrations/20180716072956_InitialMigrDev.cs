using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBTCreditControl.Repository.Migrations
{
    public partial class InitialMigrDev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorporateStatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateStatusMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    ErrorDetails = table.Column<string>(nullable: true),
                    ErrorMsg = table.Column<string>(nullable: true),
                    Para = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Branch = table.Column<string>(maxLength: 500, nullable: true),
                    BranchLocation = table.Column<string>(maxLength: 500, nullable: true),
                    CityName = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Dimension = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    EmpCode = table.Column<string>(maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "1"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Password = table.Column<string>(maxLength: 20, nullable: true),
                    UserType = table.Column<string>(maxLength: 100, nullable: true),
                    fk_SupervisorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_fk_SupervisorId",
                        column: x => x.fk_SupervisorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Corporate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: true),
                    Balance = table.Column<decimal>(nullable: true),
                    Blocked = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CustomerGroup = table.Column<string>(maxLength: 200, nullable: true),
                    CustomerType = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    Location = table.Column<string>(maxLength: 200, nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 300, nullable: true),
                    No_ = table.Column<string>(maxLength: 200, nullable: true),
                    OstSubmittedBy = table.Column<int>(nullable: true),
                    OstSubmittedFlag = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    OstSubmittedOn = table.Column<DateTime>(nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    PostingGroup = table.Column<string>(maxLength: 200, nullable: true),
                    fk_LocationId = table.Column<int>(nullable: false),
                    fromDate = table.Column<DateTime>(nullable: true),
                    toDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corporate_User_OstSubmittedBy",
                        column: x => x.OstSubmittedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporate_LocationMaster_fk_LocationId",
                        column: x => x.fk_LocationId,
                        principalTable: "LocationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FK_LocationId = table.Column<int>(nullable: false),
                    FK_UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLocation_LocationMaster_FK_LocationId",
                        column: x => x.FK_LocationId,
                        principalTable: "LocationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLocation_User_FK_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCorporate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    ModifiedBy = table.Column<int>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    fk_CorporateId = table.Column<int>(nullable: true),
                    fk_UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCorporate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCorporate_Corporate_fk_CorporateId",
                        column: x => x.fk_CorporateId,
                        principalTable: "Corporate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCorporate_User_fk_UserId",
                        column: x => x.fk_UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCorporateAction",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CallNote = table.Column<string>(maxLength: 5000, nullable: true),
                    CurrentStatus = table.Column<bool>(nullable: true, defaultValueSql: "1"),
                    FK_CorporateId = table.Column<int>(nullable: true),
                    FK_CorporateStatusMasterId = table.Column<int>(nullable: true),
                    PromiseAmount = table.Column<decimal>(nullable: true),
                    PromiseDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true, defaultValueSql: "getdate()"),
                    UserUpdatedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCorporateAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCorporateAction_Corporate_FK_CorporateId",
                        column: x => x.FK_CorporateId,
                        principalTable: "Corporate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCorporateAction_CorporateStatusMaster_FK_CorporateStatusMasterId",
                        column: x => x.FK_CorporateStatusMasterId,
                        principalTable: "CorporateStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCorporateAction_User_UserUpdatedById",
                        column: x => x.UserUpdatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Corporate_OstSubmittedBy",
                table: "Corporate",
                column: "OstSubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Corporate_fk_LocationId",
                table: "Corporate",
                column: "fk_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_fk_SupervisorId",
                table: "User",
                column: "fk_SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCorporate_fk_CorporateId",
                table: "UserCorporate",
                column: "fk_CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCorporate_fk_UserId",
                table: "UserCorporate",
                column: "fk_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCorporateAction_FK_CorporateId",
                table: "UserCorporateAction",
                column: "FK_CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCorporateAction_FK_CorporateStatusMasterId",
                table: "UserCorporateAction",
                column: "FK_CorporateStatusMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCorporateAction_UserUpdatedById",
                table: "UserCorporateAction",
                column: "UserUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_FK_LocationId",
                table: "UserLocation",
                column: "FK_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocation_FK_UserId",
                table: "UserLocation",
                column: "FK_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "UserCorporate");

            migrationBuilder.DropTable(
                name: "UserCorporateAction");

            migrationBuilder.DropTable(
                name: "UserLocation");

            migrationBuilder.DropTable(
                name: "Corporate");

            migrationBuilder.DropTable(
                name: "CorporateStatusMaster");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "LocationMaster");
        }
    }
}
