using Microsoft.AspNetCore.Http; 
namespace ModelsFileToUpload
{
    public class FileToUpload
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Data = new byte[0];
        public required IFormFile UploadedFile { get; set; } 
    }
}   