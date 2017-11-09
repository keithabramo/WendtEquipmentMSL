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
            IEnumerable<WorkOrderPriceRow> workOrderPriceRecords = new List<WorkOrderPriceRow>();
            using (var excelHelper = new ExcelDataReaderHelper(filePath))
            {
                workOrderPriceRecords = ImportHelper.GetWorkOrderPrices(excelHelper);
            }

            return workOrderPriceRecords;
        }




    }
}
