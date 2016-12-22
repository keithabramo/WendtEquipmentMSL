using System;
using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class ImportService : IImportService
    {
        private IImportEngine importEngine;

        public ImportService()
        {
            importEngine = new ImportEngine();
        }

        public IEnumerable<string> GetSheets(ImportBO importBO)
        {
            var sheets = importEngine.GetSheets(importBO.File);

            return sheets;
        }

        public IEnumerable<string> Import(ImportBO userBO)
        {
            throw new NotImplementedException();
        }
    }
}
