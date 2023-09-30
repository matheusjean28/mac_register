using ReadCsvFuncs;
using DeviceContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobsServices
{
    public class BackgroundJobs
    {
        private readonly DeviceDb _db;

        public BackgroundJobs(DeviceDb db)
        {
            _db = db;
        }

        public async Task<bool> ProcessCsvInBackground(IFormFile file)
        {
            using Stream stream = file.OpenReadStream();
            var readCsvComponent = new ReadCsv();
            var macList = await readCsvComponent.ReadCsvItens(file, _db);

            return macList != null;
        }
    }
}
