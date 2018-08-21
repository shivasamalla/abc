using Microsoft.EntityFrameworkCore;
using RBTCreditControl.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RBTCreditControl.Repository
{
    public class RBTCreditControl_Context : DbContext
    {
        public DbSet<LocationMaster> LocationMaster { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CorporateMaster> CorporateMaster { get; set; }
        public DbSet<SubmitedCorporate> SubmitedCorporate { get; set; }
        public DbSet<UserLocation> UserLocation { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<CorporateStatusMaster> StatusMaster { get; set; }

        public DbSet<UserCorporateAction> UserCorporateAction { get; set; }

        public RBTCreditControl_Context(DbContextOptions<RBTCreditControl_Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CorporateMaster>().HasOne(x => x.LocationMaster).WithMany(y => y.lstCorporateMaster).HasForeignKey(x => x.fk_LocationId);

            modelBuilder.Entity<SubmitedCorporate>().HasOne(x => x.CorporateMaster).WithMany(y => y.lstSubmitedCorporate).HasForeignKey(x => x.FK_CorporateMasterId);
            //modelBuilder.Entity<Corporate>().HasOne(x => x.User).WithMany(x => x.LstCorporates).HasForeignKey(x => x.OstSubmittedBy);
            // modelBuilder.Entity<Corporate>().HasMany(x => x.lstUserCorporateAction).WithOne(x => x.Corporate).HasForeignKey(x => x.FK_CorporateId);



            modelBuilder.Entity<UserLocation>().HasOne(x => x.User).WithMany(x => x.lstUserLocation).HasForeignKey(x => x.FK_UserId);
            modelBuilder.Entity<UserLocation>().HasOne(x => x.Location).WithMany(x => x.lstUserLocation).HasForeignKey(x => x.FK_LocationId);

            modelBuilder.Entity<UserCorporate>().HasOne(x => x.User).WithMany(x => x.lstUserCorporate).HasForeignKey(x => x.fk_UserId);
            modelBuilder.Entity<UserCorporate>().HasOne(x => x.CorporateMaster).WithMany(x => x.lstUserCorporate).HasForeignKey(x => x.fk_CorporateId);

            modelBuilder.Entity<UserCorporateAction>().HasOne(x => x.SubmitedCorporate).WithMany(x => x.lstUserCorporateAction).HasForeignKey(x => x.FK_SubmitedCorporateId);
            modelBuilder.Entity<UserCorporateAction>().HasOne(x => x.CorporateStatusMaster).WithMany(x => x.lstUserCorporateAction).HasForeignKey(x => x.FK_CorporateStatusMasterId);

            modelBuilder.Entity<UserCorporateAction>().HasOne(x => x.UserUpdatedBy).WithMany(x => x.lstUserCorporateAction).HasForeignKey(x => x.UpdatedBy);

            //modelBuilder.Entity<LocationCorporate>().HasOne(x => x.LocationMaster).WithMany(y => y.lstLocationCorporate).HasForeignKey(x => x.fk_LocationId).OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<LocationCorporate>().HasOne(x => x.Corporate).WithMany(y => y.lstLocationCorporate).HasForeignKey(x => x.fk_CorporateId).OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<LocationCorporate>().HasOne(x => x.Corporate).WithMany(x => x.lstLocationCorporate).HasForeignKey(x => x.fk_CorporateId);
            //modelBuilder.Entity<LocationCorporate>().HasOne(x => x.LocationMaster).WithMany(x => x.lstLocationCorporate).HasForeignKey(x => x.fk_LocationId);

            // modelBuilder.Entity<User>().HasOne(x => x.LocationMaster).WithMany(y => y.lstUser).HasForeignKey(x => x.fk_LocationId);

            //modelBuilder.Entity<UserCorporate>().HasOne(x => x.Corporate).WithMany(x => x.lstUserCorporate).HasForeignKey(x => x.fk_CorporateId);
            //modelBuilder.Entity<UserCorporate>().HasOne(x => x.User).WithMany(x => x.lstUserCorporate).HasForeignKey(x => x.fk_UserId);


            modelBuilder.Entity<LocationMaster>()
                .Property(x => x.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserCorporateAction>()
                .Property(x => x.CurrentStatus).HasDefaultValueSql("1");
            modelBuilder.Entity<UserCorporateAction>()
              .Property(x => x.UpdatedOn).HasDefaultValueSql("getdate()");

            
            modelBuilder.Entity<CorporateMaster>()
               .Property(x => x.UpdateFlag).HasDefaultValueSql("0");
            modelBuilder.Entity<UserCorporate>()
                .Property(x => x.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>()
               .Property(x => x.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ErrorLog>()
              .Property(x => x.CreatedOn).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>()
                .Property(x => x.IsActive).HasDefaultValueSql("1");

            #region Unique Contstarin
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasIndex(e => e.EmpCode).IsUnique();
            //});
            //modelBuilder.Entity<LocationMaster>(entity =>
            //{
            //    entity.HasIndex(e => e.Name).IsUnique();
            //});
            //modelBuilder.Entity<Corporate>(entity =>
            //{
            //    entity.HasIndex(e => e.No_).IsUnique();
            //});
            #endregion
        }
    }
}
