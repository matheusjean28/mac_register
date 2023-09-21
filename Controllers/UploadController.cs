using Microsoft.AspNetCore.Mvc;
using ModelsFileToUpload;
using DeviceContext;
using Microsoft.EntityFrameworkCore;
namespace ControllerUpload
{
    [ApiController]
    [Route("upload")]
    public class UploadController : ControllerBase 
    {

        private readonly DeviceDb _db;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFile");

        public UploadController(DeviceDb db)
        {
            _db = db;
            CheckFolderExist();
        }

        public static void CheckFolderExist()
        {
            var _currentDirectory = Directory.GetCurrentDirectory();
            var _uploadPath = Path.Combine(_currentDirectory, "UploadedFile");

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileToUpload>>> GetUploads()
        {
            var uploads = await _db.FilesUploads.ToListAsync();

            if (uploads.Count == 0)
            {
                return NotFound("No upload items found.");
            }

            return Ok(uploads);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FileToUpload>> GetUploadsById(int Id)
        {
           var itemById = await _db.FilesUploads.FindAsync(Id);

           if(itemById == null )
           {
             return NotFound("item not found");
           }

           return Ok(itemById);
        }


        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File could not be empty.");
            }

            var filePath = Path.Combine(_uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            byte[] fileData;
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }
            }
            var fileToUpload = new FileToUpload
            {
                Name = file.FileName,
                Data = fileData
            };
            _db.FilesUploads.Add(fileToUpload);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(UploadFile), new { id = fileToUpload.Id }, new { Name = file.FileName });
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