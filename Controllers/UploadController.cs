using Microsoft.AspNetCore.Mvc;
using ModelsFileToUpload;
using DeviceContext;
using Microsoft.EntityFrameworkCore;
using ReadCsvFuncs;
using MethodsFuncs;

namespace ControllerUpload
{
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase
    {

        private readonly DeviceDb _db;

        public UploadController(DeviceDb db)
        {
            _db = db;
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


        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            Methods methods = new();
            if (file == null || file.Length == 0)
            {
                return BadRequest("File could not be empty.");
            }
            var FileFormat = file.FileName;

            if (methods.CheckFileExtension(FileFormat))
            {
                using Stream stream = file.OpenReadStream();
                var readCsvComponent = new ReadCsv();
                var macList = await readCsvComponent.ReadCsvItens(file, _db);

                if (macList != null)
                {
                    return Ok($"We recive your File: {file.FileName}, we are working in!");

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