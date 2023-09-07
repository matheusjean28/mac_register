using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using DeviceContext;
using System.Threading.Tasks;
using DeviceModel;
using Microsoft.EntityFrameworkCore;
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

      
        [HttpPost]
        public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile>? files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest();

            }
            List<byte[]> Data = new();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    var device = new Device
                    {
                        Data = stream.ToArray()
                    };
                    _db.Devices.Add(device);
                }
            }
            await _db.SaveChangesAsync();
            return Ok($"Your file has been saved!");
        }
    }
}