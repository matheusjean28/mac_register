using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsFileToUpload;
using System.Collections.Generic;
using System.IO;
using DeviceContext;
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



        [HttpPost("upload")]
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
                Data = fileData
            };
            _db.FilesUploads.Add(fileToUpload);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(UploadFile), new { id = fileToUpload.Id }, new { Name = file.FileName });



        }
    }
}