
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
        private IUserEngine userEngine;
        private IPriorityEngine priorityEngine;

        public ImportService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            importEngine = new ImportEngine();
            hardwareCommercialCodeEngine = new HardwareCommercialCodeEngine(dbContext);
            userEngine = new UserEngine(dbContext);
            priorityEngine = new PriorityEngine(dbContext);
        }

        public string SaveFile(byte[] file)
        {
            return importEngine.SaveFile(file);
        }


        public IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO)
        {
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll();

            var import = new EquipmentImport
            {
                //DrawingNumber = importBO.DrawingNumber,
                Equipment = importBO.Equipment,
                FilePaths = importBO.FilePaths,
                hardwareCommercialCodes = hardwareCommercialCodes.Select(x => new HardwareCommercialCodeImport
                {
                    CommodityCode = x.CommodityCode,
                    Description = x.Description,
                    PartNumber = x.PartNumber
                }).ToList(),
                PriorityId = importBO.PriorityId,

                QuantityMultiplier = importBO.QuantityMultiplier,
                WorkOrderNumber = importBO.WorkOrderNumber,
            };

            var equipmentRows = importEngine.GetEquipment(import);


            var equipmentBOs = equipmentRows.Select(x => new EquipmentBO
            {
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                PriorityId = x.PriorityId,
                Quantity = x.Quantity,
                ReleaseDate = x.ReleaseDate,
                ShippingTagNumber = x.ShippingTagNumber,
                UnitWeight = x.UnitWeight,
                WorkOrderNumber = x.WorkOrderNumber
            });

            return equipmentBOs.ToList();
        }

        public IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(string filePath)
        {
            var workOrderPriceRows = importEngine.GetWorkOrderPrices(filePath);

            var workOrderPriceBOs = workOrderPriceRows.Select(x => new WorkOrderPriceBO
            {
                CostPrice = x.CostPrice,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber
            });

            return workOrderPriceBOs.ToList();
        }

        public IEnumerable<EquipmentBO> GetRawEquipmentImport(string filePath)
        {
            var equipmentRows = importEngine.GetRawEquipment(filePath);

            var equipmentBOs = equipmentRows.Select(x => new EquipmentBO
            {
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                PriorityNumber = x.PriorityNumber,
                Quantity = x.Quantity,
                ReleaseDate = x.ReleaseDate,
                ShippingTagNumber = x.ShippingTagNumber,
                UnitWeight = x.UnitWeight,
                WorkOrderNumber = x.WorkOrderNumber,
                CountryOfOrigin = x.CountryOfOrigin,
                HTSCode = x.HTSCode,
                Notes = x.Notes,
                ReadyToShip = x.ReadyToShip,
                ShippedFrom = x.ShippedFrom
            });

            return equipmentBOs.ToList();
        }
    }
}
