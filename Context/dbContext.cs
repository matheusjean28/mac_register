using Microsoft.EntityFrameworkCore;
using DeviceModel;
using  ModelsFileToUpload;
using MacToDatabaseModel;

namespace DeviceContext;

public class DeviceDb : DbContext
{
    public DeviceDb(DbContextOptions<DeviceDb> options)
        : base(options) { }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<FileToUpload> FilesUploads => Set<FileToUpload>();
    public DbSet<MacToDatabase> MacstoDbs => Set<MacToDatabase>();
    

}

