using AutoMapper;
using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class ImportService : IImportService
    {
        private IImportEngine importEngine;

        public ImportService()
        {
            importEngine = new ImportEngine();
        }

        public ImportBO GetSheets(byte[] file)
        {
            var import = importEngine.GetSheets(file);

            var importBO = Mapper.Map<ImportBO>(import);

            return importBO;
        }

        public IEnumerable<EquipmentBO> GetEquipmentImport(ImportBO importBO)
        {
            var import = Mapper.Map<Import>(importBO);

            var equipmentRows = importEngine.GetEquipment(import);

            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(equipmentRows);

            return equipmentBOs;
        }
    }
}
