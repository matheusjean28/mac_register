using MacToDatabaseModel;
using MainDatabaseContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ModelsFileToUpload;
namespace SavePureCsvOnDatabaseServices
{
    public class SavePureCsvOnDatabase
    {
        private readonly MainDatabase _db;
        public SavePureCsvOnDatabase( MainDatabase db)
        {
            _db = db;
        }

        public async Task<Boolean> SaveOnDatabase()
        {
            MacToDatabase device = new(){
                Model = "modelo de teste do novo server",
                Mac= "123456789",
                Problem = false, 
                RemoteAccess = true
            };
            try
            {
             await _db.DevicesToMain.AddAsync(device);
             await _db.SaveChangesAsync();
             return true;   
            }
            catch (System.Exception)
            {
             return false;   
                
                throw;
            }
        }


    }
}