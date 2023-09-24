using System;
using System.IO;
using MethodsInterfaces;
namespace MethodsFuncs

{
    public class Methods : IMethods
    {
         private readonly string _uploadPath;

        public Methods()
        {
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFile");
        }

        public void CheckAndCreateFolderIfNotExist()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }  }
}