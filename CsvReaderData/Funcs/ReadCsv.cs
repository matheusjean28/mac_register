using MacToDatabaseModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using DeviceCsv.Model;
using Read.Interfaces;
using DeviceContext;
namespace ReadCsvFuncs
{
    public class ReadCsv : IRead
    {
        private readonly string folderName = "Temp";
        private readonly string folderPath = Directory.GetCurrentDirectory();



        public async Task<IEnumerable<MacToDatabase>> ReadCsvItens(IFormFile file,DeviceDb db)
        {
            var _folderPath = folderPath;
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            _folderPath = Path.GetFullPath(folderName);


            List<MacToDatabase> macList = new();


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),

            };

            using var reader = new StreamReader((Stream)file);
            using var csv = new CsvReader(reader, config);


            var records = csv.GetRecordsAsync<Device>();

            await foreach (var device in records)
            {
                try
                {
                    MacToDatabase deviceItem = new();
                    static bool IsValidMacAddress(string mac)
                    {
                        string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
                        return Regex.IsMatch(mac, pattern);
                    }
                    if (IsValidMacAddress(device.Mac))
                    {
                        deviceItem.Mac = device.Mac;
                    }
                    else
                    {
                        string errorMessage = $"[Error Occurred at {DateTime.Now}] - Invalid Model: {device.Model}, MAC: {device.Mac}";

                        await File.AppendAllTextAsync(Path.Combine(_folderPath, "Error.csv"), errorMessage);
                        continue;
                    }

                    if (device.Model.Length <= 0 || device.Model.Length >= 99)
                    {
                        string errorMessage = $"[Error Occurred at {DateTime.Now}] - Invalid Model: {device.Model}, MAC: {device.Mac}";

                        await File.AppendAllTextAsync(Path.Combine(_folderPath, "Error.csv"), errorMessage);
                        continue;
                    }
                    else
                    {
                        deviceItem.Model = device.Model;
                    }
                    macList.Add(deviceItem);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"[Error Occurred at {DateTime.Now}] - {ex.Message}";
                    await File.AppendAllTextAsync(Path.Combine(_folderPath, "Error.csv"), errorMessage);
                    continue;
                }
            }
            foreach (var item in macList)
            {
                Console.WriteLine($"Mac é:  >>  {item.Mac}");
                await db.MacstoDbs.AddAsync(item);

            }
            await db.SaveChangesAsync();
            return macList;
        }
    }
};