using DeviceModel;
using MacSave.Models.Categories;
using MacSave.Models.Categories.Models_of_Devices;
using MacSave.Models.SinalHistory;
using MacToDatabaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;
using ModelsFileToUpload;

namespace DeviceContext
{
    public class DeviceDb : DbContext
    {
        public DeviceDb(DbContextOptions<DeviceDb> options)
            : base(options) { }

        public DbSet<DeviceCreate> Devices => Set<DeviceCreate>();
        public DbSet<ProblemTreatWrapper> Problems => Set<ProblemTreatWrapper>();
        public DbSet<UsedAtWrapper> UsedAtClient => Set<UsedAtWrapper>();
        public DbSet<SinalHistory> Histories => Set<SinalHistory>();

        //items to upload and threat data
        public DbSet<FileToUpload> FilesUploads => Set<FileToUpload>();
        public DbSet<MacToDatabase> MacstoDbs => Set<MacToDatabase>();

        //items from device categories
        public DbSet<Maker> Makers => Set<Maker>();
        public DbSet<DeviceCategory> DeviceCategories=> Set<DeviceCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceCreate>()
            .HasMany(e => e.Problems)
            .WithOne(e => e.DeviceCreate)
            .HasForeignKey(e => e.DeviceId)
            .IsRequired();

            modelBuilder.Entity<DeviceCreate>()
             .HasMany(e => e.UsedAtClients)
            .WithOne(e => e.DeviceCreate)
            .HasForeignKey(e => e.DeviceId)
            .IsRequired();

            modelBuilder.Entity<DeviceCreate>()
            .HasMany(e => e.SinalHistory)
            .WithOne(e => e.DeviceCreate)
            .HasForeignKey(e => e.DeviceId)
            .IsRequired();

             modelBuilder.Entity<Maker>()
            .Property( e => e.MakerId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<DeviceCategory>()
            .HasMany(e => e.Devices)
            .WithOne(e => e.DeviceCategory)
            .HasForeignKey(e => e.DeviceCategoryId)
            .IsRequired();

            // modelBuilder.Entity<Maker>()
            // .HasMany(m => m.DeviceCategories)
            // .WithOne(m => m.Maker)
            // .HasForeignKey(m => m.MakerId)
            // .IsRequired();


        }

        
    }
}
