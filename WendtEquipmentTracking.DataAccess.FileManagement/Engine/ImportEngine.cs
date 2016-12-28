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
        public IEnumerable<ImportRow> GetEquipment(Import import)
        {

            var excelHelper = new ExcelDataReaderHelper(import.FileName);

            var importData = new List<ImportRow>();
            foreach (var sheet in import.Sheets)
            {
                var importSheet = ImportHelper.GetImportRecordsForSheet(sheet, excelHelper);
                importData.AddRange(importSheet);
            }

            return importData;
        }

        public Import GetSheets(byte[] importFile)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllBytes(tempFile, importFile);

            var helper = new ExcelDataReaderHelper(tempFile);


            var import = new Import
            {
                Sheets = helper.WorksheetNames.ToList(),
                FileName = tempFile
            };

            return import;
        }
    }
}
