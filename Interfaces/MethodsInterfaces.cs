using DeviceContext;

namespace MethodsInterfaces
{
    public interface IMethods
    {
        void CheckAndCreateFolderIfNotExist();
        public bool CheckFileExtension(string _fileName);
        IAsyncEnumerable<bool> CheckIfMacAlreadyExists(DeviceDb db, string _macDevice);
    }

}