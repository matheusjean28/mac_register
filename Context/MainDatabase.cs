using Microsoft.EntityFrameworkCore;
using MacToDatabaseModel;

namespace MainDatabaseContext
{
    public class MainDatabase : DbContext
    {
        public MainDatabase(DbContextOptions<MainDatabase> options)
        : base(options) { }
        public DbSet<MacToDatabase> DevicesToMain => Set<MacToDatabase>();

    }
}