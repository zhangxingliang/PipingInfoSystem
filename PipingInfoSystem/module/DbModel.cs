namespace PipingInfoSystem.module
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using model;
    public partial class DbModel : DbContext
    {
        public DbModel()
            : base("name=PipingInfoDb")
        {
        }

        public virtual DbSet<PipingDetectionInfo> PipingDetectionInfoes { get; set; }
        public virtual DbSet<PipingPictureInfo> PipingPictureInfoes { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.KeyID)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.PipingID)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.VideoFile)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.StartWellNo)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.EndWellNo)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.LayingYear)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.StartPointDepth)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.EndPointDepth)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.TubulationType)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.TubulationMaterial)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.TubulationDiameter)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.DetectionDirect)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.TubulationLength)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.DetectionAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.DetectionTime)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.AddPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.DetectionLength)
                .IsUnicode(false);

            modelBuilder.Entity<PipingDetectionInfo>()
                .Property(e => e.DetectionFun)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.KeyID)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.PipingID)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.PictureID)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.PictureFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.PipingInternalState)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.Grade)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.Score)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.DefectCode)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.Distance)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.AddPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<PipingPictureInfo>()
                .Property(e => e.PictureRemark)
                .IsUnicode(false);
            modelBuilder.Entity<UserInfo>()
              .Property(e => e.KeID)
              .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.TrueName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.AuditUser)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.UserID)
                .IsUnicode(false);
        }
    }
}
