﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RBTCreditControl.Repository;
using System;

namespace RBTCreditControl.Repository.Migrations
{
    [DbContext(typeof(RBTCreditControl_Context))]
    [Migration("20180716072956_InitialMigrDev")]
    partial class InitialMigrDev
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RBTCreditControl.Entity.Corporate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("Amount");

                    b.Property<decimal?>("Balance");

                    b.Property<string>("Blocked")
                        .HasMaxLength(200);

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("CustomerGroup")
                        .HasMaxLength(200);

                    b.Property<string>("CustomerType")
                        .HasMaxLength(200);

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("Location")
                        .HasMaxLength(200);

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(300);

                    b.Property<string>("No_")
                        .HasMaxLength(200);

                    b.Property<int?>("OstSubmittedBy");

                    b.Property<int>("OstSubmittedFlag")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<DateTime?>("OstSubmittedOn");

                    b.Property<string>("Phone")
                        .HasMaxLength(50);

                    b.Property<string>("PostingGroup")
                        .HasMaxLength(200);

                    b.Property<int>("fk_LocationId");

                    b.Property<DateTime?>("fromDate");

                    b.Property<DateTime?>("toDate");

                    b.HasKey("Id");

                    b.HasIndex("OstSubmittedBy");

                    b.HasIndex("fk_LocationId");

                    b.ToTable("Corporate");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.CorporateStatusMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("CorporateStatusMaster");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.ErrorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("ErrorDetails");

                    b.Property<string>("ErrorMsg");

                    b.Property<string>("Para");

                    b.Property<string>("Uri")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("ErrorLog");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.LocationMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Branch")
                        .HasMaxLength(500);

                    b.Property<string>("BranchLocation")
                        .HasMaxLength(500);

                    b.Property<string>("CityName")
                        .HasMaxLength(500);

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Dimension")
                        .HasMaxLength(500);

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("LocationMaster");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("EmpCode")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("1");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasMaxLength(20);

                    b.Property<string>("UserType")
                        .HasMaxLength(100);

                    b.Property<int?>("fk_SupervisorId");

                    b.HasKey("Id");

                    b.HasIndex("fk_SupervisorId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserCorporate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<int?>("fk_CorporateId");

                    b.Property<int?>("fk_UserId");

                    b.HasKey("Id");

                    b.HasIndex("fk_CorporateId");

                    b.HasIndex("fk_UserId");

                    b.ToTable("UserCorporate");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserCorporateAction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CallNote")
                        .HasMaxLength(5000);

                    b.Property<bool?>("CurrentStatus")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("1");

                    b.Property<int?>("FK_CorporateId");

                    b.Property<int?>("FK_CorporateStatusMasterId");

                    b.Property<decimal?>("PromiseAmount");

                    b.Property<DateTime?>("PromiseDate");

                    b.Property<int?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int?>("UserUpdatedById");

                    b.HasKey("Id");

                    b.HasIndex("FK_CorporateId");

                    b.HasIndex("FK_CorporateStatusMasterId");

                    b.HasIndex("UserUpdatedById");

                    b.ToTable("UserCorporateAction");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FK_LocationId");

                    b.Property<int>("FK_UserId");

                    b.HasKey("Id");

                    b.HasIndex("FK_LocationId");

                    b.HasIndex("FK_UserId");

                    b.ToTable("UserLocation");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.Corporate", b =>
                {
                    b.HasOne("RBTCreditControl.Entity.User", "User")
                        .WithMany("LstCorporates")
                        .HasForeignKey("OstSubmittedBy");

                    b.HasOne("RBTCreditControl.Entity.LocationMaster", "LocationMaster")
                        .WithMany("lstCorporate")
                        .HasForeignKey("fk_LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RBTCreditControl.Entity.User", b =>
                {
                    b.HasOne("RBTCreditControl.Entity.User", "Supervisor")
                        .WithMany()
                        .HasForeignKey("fk_SupervisorId");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserCorporate", b =>
                {
                    b.HasOne("RBTCreditControl.Entity.Corporate", "Corporate")
                        .WithMany("lstUserCorporate")
                        .HasForeignKey("fk_CorporateId");

                    b.HasOne("RBTCreditControl.Entity.User", "User")
                        .WithMany("lstUserCorporate")
                        .HasForeignKey("fk_UserId");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserCorporateAction", b =>
                {
                    b.HasOne("RBTCreditControl.Entity.Corporate", "Corporate")
                        .WithMany("lstUserCorporateAction")
                        .HasForeignKey("FK_CorporateId");

                    b.HasOne("RBTCreditControl.Entity.CorporateStatusMaster", "CorporateStatusMaster")
                        .WithMany("lstUserCorporateAction")
                        .HasForeignKey("FK_CorporateStatusMasterId");

                    b.HasOne("RBTCreditControl.Entity.User", "UserUpdatedBy")
                        .WithMany("lstUserCorporateAction")
                        .HasForeignKey("UserUpdatedById");
                });

            modelBuilder.Entity("RBTCreditControl.Entity.UserLocation", b =>
                {
                    b.HasOne("RBTCreditControl.Entity.LocationMaster", "Location")
                        .WithMany("lstUserLocation")
                        .HasForeignKey("FK_LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RBTCreditControl.Entity.User", "User")
                        .WithMany("lstUserLocation")
                        .HasForeignKey("FK_UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}