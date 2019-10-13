using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.FileManagement.Helper;

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
