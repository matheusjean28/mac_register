using DeviceContext;
using Microsoft.EntityFrameworkCore;
using ModelsFileToUpload;
namespace SavePureCsvOnDatabaseServices
{
    public class SavePureCsvOnDatabase
    {
        private readonly DeviceDb _db;
        public SavePureCsvOnDatabase(Uri uri,DeviceDb db)
        {
            _db = db;
        }

        
    }
}