using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.FileManagement.Helper;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class ImportEngine : IImportEngine
    {
        public string SaveFile(byte[] file)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, file);

            return tempFile;
        }

        public IEnumerable<EquipmentRow> GetEquipment(EquipmentImport import)
        {
            var equipmentData = ImportHelper.GetEquipment(import);

            return equipmentData;
        }

        public IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(string filePath)
        {
            var workOrderPriceRecords = ImportHelper.GetWorkOrderPrices(filePath);
            
            return workOrderPriceRecords;
        }

        public IEnumerable<RawEquipmentRow> GetRawEquipment(string filePath)
        {
            return ImportHelper.GetRawEquipment(filePath);
        }

        public IEnumerable<PriorityRow> GetPriorities(string filePath)
        {
            return ImportHelper.GetPriorities(filePath);
        }
    }
}
