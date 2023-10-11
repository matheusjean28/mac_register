using Microsoft.AspNetCore.Mvc;
using ModelsFileToUpload;
using DeviceContext;
using Microsoft.EntityFrameworkCore;
using MethodsFuncs;
using BackgroundCallProcessCsvServices;

namespace ControllerUpload
{
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase
    {
        private readonly HttpClient httpClient;
        private readonly DeviceDb _db;
        public UploadController(DeviceDb db)
        {
            _db = db;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/recive-process")
            };
        }

        [HttpGet("getService")]
        public async Task TriggerCsvProcessing(int id)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"recive-process?id={id}", null);

            if (response.IsSuccessStatusCode)
            {
                var contentResponse = response.Content;
                Console.WriteLine($"{contentResponse}.");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error on processing CSV: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error on processing CSV: {ex.Message}");
        }
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetUploads()
        {
            var uploads = await _db.MacstoDbs.ToListAsync();
            if (uploads.Count == 0)
            {
                return NotFound("No upload items found.");
            }
            return Ok(uploads);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FileToUpload>> GetUploadsById(int Id)
        {
            var itemById = await _db.MacstoDbs.FindAsync(Id);
            if (itemById == null)
            {
                return NotFound("item not found");
            }
            return Ok(itemById);
        }



        //Anotation -- stap on process txt
        //post method
        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            Methods methods = new();
            BackgroundCallProcessCsv backgroundJobs = new(_db);

            if (file == null || file.Length == 0)
            {
                return BadRequest("File could not be empty.");
            }
            var FileFormat = file.FileName;

            if (methods.CheckFileExtension(FileFormat))
            {
                bool processingResult = await backgroundJobs.ProcessCsvInBackground(file);

                if (processingResult)
                {
                    return Ok($"We received your File: {file.FileName}, we are working on it!");
                }
                else
                {
                    return BadRequest("Failed to process the CSV file.");
                }
            }
            else
            {
                return BadRequest("The file must be a .Csv File.");
            }
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