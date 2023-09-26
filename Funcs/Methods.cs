using DeviceContext;
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
        }

        public bool CheckFileExtension(string _fileName)
        {
            var Format = Path.GetExtension(_fileName);
            if (Format != ".csv")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async IAsyncEnumerable<bool> CheckIfMacAlreadyExists(DeviceDb db, string _macDevice)
        {
            var _content = await db.MacstoDbs.FindAsync(_macDevice);
            if (_content != null)
            {
                yield return true; 
            }
            else
            {
                yield return false;
            }
        }
    }
}