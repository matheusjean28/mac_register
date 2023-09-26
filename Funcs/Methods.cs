using System.Text.RegularExpressions;
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

         public async Task<bool> IsValidMacAddress(DeviceDb db, string mac)
                    {
                        string pattern = @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";
                        var MatchMac = Regex.IsMatch(mac, pattern);

                        var _checkMac = await db.MacstoDbs.FindAsync(MatchMac);
                        if (_checkMac != null)
                        {
                            return true;
                        }
                        else
                        {

                            return false;
                        }
                    }
        
    }
}