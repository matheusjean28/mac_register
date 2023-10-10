using ReadCsvFuncs;
using DeviceContext;

namespace BackgroundCallProcessCsvServices
{
    public class BackgroundCallProcessCsv
    {
        private readonly DeviceDb _db;

        public BackgroundCallProcessCsv(DeviceDb db)
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
