using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class AttachmentEngine : IFileEngine
    {
        public void SaveFile(string fiplePath, byte[] file)
        {
            File.WriteAllBytes(fiplePath, file);
        }
    }
}
