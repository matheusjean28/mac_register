using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace ControllerUpload
{
    [ApiController]
    [Route("upload")]

    public class UploadController : ControllerBase
    {
        public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest();
            }

            List<byte[]> data = new();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        data.Add(stream.ToArray());
                    }
                }
            }
            return File(data[0], files.FirstOrDefault().ContentType, "myfile.txt");
        }
    }
}