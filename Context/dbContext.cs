using Microsoft.EntityFrameworkCore;
using DeviceModel;
namespace DeviceContext;

public class DeviceDb : DbContext
{
    public DeviceDb(DbContextOptions<DeviceDb> options)
        : base(options) { }

    public DbSet<Device> Devices => Set<Device>();
}

