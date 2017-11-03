using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class ImportService : IImportService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IImportEngine importEngine;
        private IHardwareCommercialCodeEngine hardwareCommercialCodeEngine;

        public ImportService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            importEngine = new ImportEngine();
            hardwareCommercialCodeEngine = new HardwareCommercialCodeEngine(dbContext);
        }

        public string SaveEquipmentFile(byte[] file)
        {
            return importEngine.SaveEquipmentFile(file);
        }


        public IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO)
        {
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll().ToList();

            var import = Mapper.Map<EquipmentImport>(importBO);
            import.hardwareCommercialCodes = Mapper.Map<IEnumerable<HardwareCommercialCodeImport>>(hardwareCommercialCodes);

            var equipmentRows = importEngine.GetEquipment(import);

            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(equipmentRows);

            return equipmentBOs;
        }

        public IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(byte[] file)
        {
            var workOrderPriceRows = importEngine.GetWorkOrderPrices(file);

            var workOrderPriceBOs = Mapper.Map<IEnumerable<WorkOrderPriceBO>>(workOrderPriceRows);

            return workOrderPriceBOs;
        }
    }
}
