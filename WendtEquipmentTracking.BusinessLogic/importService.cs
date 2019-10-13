﻿
using System.Collections.Generic;
using System.IO;
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
            var tempFile = Path.GetTempFileName();

            importEngine.SaveFile(tempFile, file);

            return tempFile;
        }


        public IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO)
        {
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll();

            var import = new EquipmentImport
            {
                //DrawingNumber = importBO.DrawingNumber,
                Equipment = importBO.Equipment,
                FilePaths = importBO.FilePaths,
                hardwareCommercialCodes = hardwareCommercialCodes.Select(x => new HardwareCommercialCodeRow
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

        public IEnumerable<PriorityBO> GetPrioritiesImport(string filePath)
        {
            var priorityRows = importEngine.GetPriorities(filePath);

            var priorityBOs = priorityRows.Select(x => new PriorityBO
            {
                ContractualShipDate = x.ContractualShipDate,
                DueDate = x.DueDate,
                EndDate = x.EndDate,
                EquipmentName = x.EquipmentName,
                PriorityNumber = x.PriorityNumber
            });

            return priorityBOs.ToList();
        }

        public IEnumerable<VendorBO> GetVendorsImport(string filePath)
        {
            var vendorRows = importEngine.GetVendors(filePath);

            var vendorBOs = vendorRows.Select(x => new VendorBO
            {
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                Name = x.Name,
                PhoneFax = x.PhoneFax
            });

            return vendorBOs.ToList();
        }

        public IEnumerable<BrokerBO> GetBrokersImport(string filePath)
        {
            var brokerRows = importEngine.GetBrokers(filePath);

            var brokerBOs = brokerRows.Select(x => new BrokerBO
            {
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                Name = x.Name,
                PhoneFax = x.PhoneFax
            });

            return brokerBOs.ToList();
        }

        public IEnumerable<HardwareCommercialCodeBO> GetHardwareCommercialCodesImport(string filePath)
        {
            var hardwareCommercialCodeRows = importEngine.GetHardwareCommercialCodes(filePath);

            var hardwareCommercialCodeBOs = hardwareCommercialCodeRows.Select(x => new HardwareCommercialCodeBO
            {
                PartNumber = x.PartNumber,
                Description = x.Description,
                CommodityCode = x.CommodityCode
            });

            return hardwareCommercialCodeBOs.ToList();
        }
    }
}
