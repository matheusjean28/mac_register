using System;
using System.IO;

namespace MethodsInterfaces
{
    public interface IMethods
    {
        void CheckAndCreateFolderIfNotExist();
        public bool CheckFileExtension(string _fileName);
    }
    
}