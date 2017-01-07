using Excel.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.FileManagement.Helper;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class ImportEngine : IImportEngine
    {
        public IEnumerable<EquipmentRow> GetEquipment(Import import)
        {

            var excelHelper = new ExcelDataReaderHelper(import.FileName);

            var importData = new List<EquipmentRow>();
            foreach (var sheet in import.Sheets)
            {
                var sheetData = ImportHelper.GetEquipmentFromSheet(sheet, excelHelper);
                importData.AddRange(sheetData);
            }

            return importData;
        }

        public IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(byte[] importFile)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, importFile);

            var excelHelper = new ExcelDataReaderHelper(tempFile);

            var workOrderPriceRecords = ImportHelper.GetWorkOrderPrices(excelHelper);

            return workOrderPriceRecords;
        }

        public Import GetSheets(byte[] importFile)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, importFile);

            var excelHelper = new ExcelDataReaderHelper(tempFile);

            var import = new Import
            {
                Sheets = excelHelper.WorksheetNames.ToList(),
                FileName = tempFile
            };

            return import;
        }


    }
}
