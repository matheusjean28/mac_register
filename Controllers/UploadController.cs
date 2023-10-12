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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetUploads()
        {
            var uploads = await _db.MacstoDbs.ToListAsync();
            if (uploads.Count == 0 || uploads == null)
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