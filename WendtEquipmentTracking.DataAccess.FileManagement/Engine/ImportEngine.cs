using LinqToExcel;
using LinqToExcel.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class ImportEngine : IImportEngine
    {
        public IEnumerable<EquipmentRow> GetEquipment(byte[] importFile)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetSheets(byte[] importFile)
        {
            var tempParcelFile = Path.GetTempFileName();
            File.WriteAllBytes(tempParcelFile, importFile);

            var excel = new ExcelQueryFactory(tempParcelFile);
            //excel.DatabaseEngine = DatabaseEngine.Ace;

            return excel.GetWorksheetNames();
        }
    }
}
