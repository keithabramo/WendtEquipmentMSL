
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
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll();

            var import = new EquipmentImport
            {
                DrawingNumber = importBO.DrawingNumber,
                Equipment = importBO.Equipment,
                FilePath = importBO.FilePath,
                hardwareCommercialCodes = hardwareCommercialCodes.Select(x => new HardwareCommercialCodeImport
                {
                    CommodityCode = x.CommodityCode,
                    Description = x.Description,
                    PartNumber = x.PartNumber
                }).ToList(),
                Priority = importBO.Priority,
                QuantityMultiplier = importBO.QuantityMultiplier,
                WorkOrderNumber = importBO.WorkOrderNumber,
            };

            var equipmentRows = importEngine.GetEquipment(import);

            var equipmentBOs = equipmentRows.Select(x => new EquipmentBO
            {
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                Priority = x.Priority,
                Quantity = x.Quantity,
                ReleaseDate = x.ReleaseDate,
                ShippingTagNumber = x.ShippingTagNumber,
                UnitWeight = x.UnitWeight,
                WorkOrderNumber = x.WorkOrderNumber
            });

            return equipmentBOs.ToList();
        }

        public IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(byte[] file)
        {
            var workOrderPriceRows = importEngine.GetWorkOrderPrices(file);

            var workOrderPriceBOs = workOrderPriceRows.Select(x => new WorkOrderPriceBO
            {
                CostPrice = x.CostPrice,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.CostPrice,
                WorkOrderNumber = x.WorkOrderNumber
            });

            return workOrderPriceBOs.ToList();
        }
    }
}
