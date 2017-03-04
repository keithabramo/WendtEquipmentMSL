using Excel.Helper;
using System.Collections.Generic;
using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.FileManagement.Helper;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class ImportEngine : IImportEngine
    {
        public string SaveEquipmentFile(byte[] equipmentFile)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, equipmentFile);

            return tempFile;
        }

        public IEnumerable<EquipmentRow> GetEquipment(EquipmentImport import)
        {
            var equipmentData = ImportHelper.GetEquipment(import);

            return equipmentData;
        }

        public IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(byte[] importFile)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, importFile);

            var excelHelper = new ExcelDataReaderHelper(tempFile);

            var workOrderPriceRecords = ImportHelper.GetWorkOrderPrices(excelHelper);

            return workOrderPriceRecords;
        }




    }
}
