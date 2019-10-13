using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public abstract class FileEngine : IFileEngine
    {
        public void SaveFile(string filePath, byte[] file)
        {
            File.WriteAllBytes(filePath, file);
        }

        public void RemoveFile(string filePath)
        {
            File.Delete(filePath);
        }
    }
}
