using Microsoft.AspNetCore.Mvc;
using ModelsFileToUpload;
using DeviceContext;
using SavePureCsvOnDatabaseServices;
using Microsoft.EntityFrameworkCore;
using MainDatabaseContext;


namespace ControllerUpload
{
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly DeviceDb _db;
        private readonly MainDatabase _dbMain;
        public UploadController(DeviceDb db,MainDatabase dbMain  )
        {
            _db = db;
            _dbMain = dbMain;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
        }

        [HttpGet("/CallProcessCsv")]
        public async Task<ActionResult<IEnumerable<HttpClient>>> TriggerCsvProcessing(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync($"process-csv?id={id}", null);

                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"{contentResponse}");
                    return Ok($"{contentResponse}");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error on processing CSV");
                    return BadRequest($"{errorMessage}");
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Error on processing CSV");
                return BadRequest($"Error on processing CSV");
            }
        }



        [HttpPost("/CsvSave")]
        public async Task<IActionResult> SaveCsvAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File can not be empty.");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var csvFile = new FileToUpload
                {
                    Name = file.FileName,
                    Data = memoryStream.ToArray()
                };
                _db.FilesUploads.Add(csvFile);
                await _db.SaveChangesAsync();
                var FileName = csvFile.Name;
                return Ok($"{FileName} was sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error on file upload: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetUploads()
        {
            var uploads = await _db.Devices.ToListAsync();
            if (uploads.Count == 0 || uploads == null)
            {
                return NotFound("No upload items found.");
            }
            return Ok(uploads);
        }

        [HttpGet("/integer")]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetIntegerUploadsAsync()
        {
            var uploads = await _db.FilesUploads.ToListAsync();
            if (uploads.Count == 0 || uploads == null)
            {
                return NotFound("No upload items found.");
            }
            return Ok(uploads);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FileToUpload>> GetUploadsById(int Id)
        {
            var itemById = await _db.Devices.FindAsync(Id);
            if (itemById == null)
            {
                return NotFound("item not found");
            }
            return Ok(itemById);
        }



        [HttpPost("/CreateMac")]
        public async Task<IActionResult> CreateMacAsync(MainDatabase dbMain) 
        {
            SavePureCsvOnDatabase save = new(dbMain);
            if(await save.SaveOnDatabase()){
                return Ok("save with sucess");
            }
            return BadRequest("an error was ocurred");
        }

        [HttpGet("/MacMainDatabase")]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetMacMainDatabaseAsync()
        {
            var macs = await _dbMain.DevicesToMain.ToListAsync();
            if (macs.Count == 0 || macs == null)
            {
                return NotFound("No upload items found.");
            }
            return Ok(macs);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(int Id)
        {
            var DeleteID = await _db.FilesUploads.FindAsync(Id);
            if (DeleteID == null)
            {
                var itemdelete = $"The item may not exists!";
                return BadRequest(itemdelete);
            }
            _db.FilesUploads.Remove(DeleteID);
            await _db.SaveChangesAsync();

            var itemId = $"The item {DeleteID.Name} was deleted with sucess!";
            return Ok(itemId);
        }
    }
}