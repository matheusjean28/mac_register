using Microsoft.EntityFrameworkCore;
using DeviceModel;
using  ModelsFileToUpload;


namespace DeviceContext;

public class DeviceDb : DbContext
{
    public DeviceDb(DbContextOptions<DeviceDb> options)
        : base(options) { }

    public DbSet<Device> Devices => Set<Device>();
    public DbSet<FileToUpload> filesUpload => Set<FileToUpload>();
}

