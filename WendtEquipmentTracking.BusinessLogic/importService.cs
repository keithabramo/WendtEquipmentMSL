
using System;
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
            var equipmentBOs = new List<EquipmentBO>();

            var hardwareCommercialCodeBOs = hardwareCommercialCodeEngine.ListAll().Select(x => new HardwareCommercialCodeBO
            {
                CommodityCode = x.CommodityCode,
                Description = x.Description,
                PartNumber = x.PartNumber
            }).ToList();

            var equipmentRows = new List<EquipmentRow>();
            foreach (var keyValuePair in importBO.FilePaths)
            {
                //splitting these. They were joined in the ImportEquipment.js file
                var drawingNumber = keyValuePair.Key;
                var filePath = keyValuePair.Value;

                var drawingEquipmentRows = importEngine.GetEquipment(filePath).ToList();
                drawingEquipmentRows.ForEach(x => x.DrawingNumber = drawingNumber);

                equipmentRows.AddRange(drawingEquipmentRows);
            }

            foreach (var equipmentRow in equipmentRows)
            {
                var hardwareCommercialCode = hardwareCommercialCodeBOs.SingleOrDefault(h => h.PartNumber == equipmentRow.PartNumber.ToString());

                string equipmentName = string.Empty;
                if (hardwareCommercialCode != null)
                {
                    equipmentName = hardwareCommercialCode.CommodityCode;
                }
                else
                {
                    equipmentName = importBO.Equipment;
                }

                var unitWeight = equipmentRow.UnitWeight;
                if (!string.IsNullOrEmpty(equipmentName) && equipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase))
                {
                    unitWeight = .01;
                }

                //round up to .01 for 0 unit weights
                if (unitWeight == 0)
                {
                    unitWeight = .01;
                }

                var quantity = importBO.QuantityMultiplier * equipmentRow.Quantity;

                var releaseDate = DateTime.Now;

                var equipmentBO = new EquipmentBO
                {
                    Description = equipmentRow.Description,
                    DrawingNumber = equipmentRow.DrawingNumber,
                    EquipmentName = equipmentName,
                    PriorityId = importBO.PriorityId,
                    Quantity = quantity,
                    ReleaseDate = releaseDate,
                    ShippingTagNumber = equipmentRow.PartNumber,
                    UnitWeight = unitWeight,
                    WorkOrderNumber = importBO.WorkOrderNumber,
                    ShippedFrom = "WENDT" //Default to this and allow them to change
                };

                equipmentBOs.Add(equipmentBO);
            }

            return equipmentBOs.ToList();
        }

        public IEnumerable<EquipmentBO> GetEquipmentRevisionImport(EquipmentRevisionImportBO importBO)
        {
            var equipmentBOs = new List<EquipmentBO>();

            var hardwareCommercialCodeBOs = hardwareCommercialCodeEngine.ListAll().Select(x => new HardwareCommercialCodeBO
            {
                CommodityCode = x.CommodityCode,
                Description = x.Description,
                PartNumber = x.PartNumber
            }).ToList();

            var equipmentRows = importEngine.GetEquipment(importBO.FilePath).ToList();
            equipmentRows.ForEach(x => x.DrawingNumber = importBO.DrawingNumber);

            foreach (var equipmentRow in equipmentRows)
            {
                var hardwareCommercialCode = hardwareCommercialCodeBOs.SingleOrDefault(h => h.PartNumber == equipmentRow.PartNumber.ToString());

                string equipmentName = string.Empty;
                if (hardwareCommercialCode != null)
                {
                    equipmentName = hardwareCommercialCode.CommodityCode;
                }
                else
                {
                    equipmentName = importBO.Equipment;
                }

                var unitWeight = equipmentRow.UnitWeight;
                if (!string.IsNullOrEmpty(equipmentName) && equipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase))
                {
                    unitWeight = .01;
                }

                //round up to .01 for 0 unit weights
                if (unitWeight == 0)
                {
                    unitWeight = .01;
                }

                var quantity = importBO.QuantityMultiplier * equipmentRow.Quantity;

                var releaseDate = DateTime.Now;

                var equipmentBO = new EquipmentBO
                {
                    Description = equipmentRow.Description,
                    DrawingNumber = equipmentRow.DrawingNumber,
                    EquipmentName = equipmentName,
                    PriorityId = importBO.PriorityId,
                    Quantity = quantity,
                    ReleaseDate = releaseDate,
                    ShippingTagNumber = equipmentRow.PartNumber,
                    UnitWeight = unitWeight,
                    WorkOrderNumber = importBO.WorkOrderNumber,
                    ShippedFrom = "WENDT" //Default to this and allow them to change
                };

                equipmentBOs.Add(equipmentBO);
            }

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
