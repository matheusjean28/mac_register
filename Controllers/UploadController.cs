using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace ControllerUpload
{
    [ApiController]
    [Route("[upload]")]
    public class UploadController : ControllerBase
    {
        public async Task<ActionResult> Upload([FromForm] ICollection<IFormFile> files)
        {

            List<byte[]> data = new();
            foreach (var formFile in files) { 
                if(formFile.Length > 0){
                    using (var stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        data.Add(stream.ToArray());
                    }
                }
            }
            return Ok();
        }
    }
}