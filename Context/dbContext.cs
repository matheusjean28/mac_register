using DeviceModel;
using MacToDatabaseModel;
using Microsoft.EntityFrameworkCore;
using ModelsFileToUpload;
using Models.UsedAtWrapper.UsedAtWrapper;
using Model.ProblemTreatWrapper;

namespace DeviceContext
{
    public class DeviceDb : DbContext
    {
        public DeviceDb(DbContextOptions<DeviceDb> options)
            : base(options) { }

        public DbSet<DeviceCreate> Devices => Set<DeviceCreate>();
        public DbSet<FileToUpload> FilesUploads => Set<FileToUpload>();
        public DbSet<MacToDatabase> MacstoDbs => Set<MacToDatabase>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceCreate>(entity =>
            {
                entity.HasKey(d => d.DeviceId);
                entity.Property(d => d.DeviceId)
                      .ValueGeneratedOnAdd();

                entity.HasMany(d => d.Problems)
                      .WithOne(p => p.Device)
                      .HasForeignKey(p => p.DeviceId);

                entity.HasMany(d => d.UsedAtWrappers)
                      .WithOne(u => u.Device)
                      .HasForeignKey(u => u.DeviceId);
            });

            modelBuilder.Entity<ProblemTreatWrapper>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UsedAtWrapper>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<FileToUpload>(entity =>
            {
                entity.HasKey(f => f.Id);
            });

            modelBuilder.Entity<MacToDatabase>(entity =>
            {
                entity.HasKey(m => m.Id);
            });
        }
    }
}
