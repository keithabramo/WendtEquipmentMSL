
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class EquipmentService : IEquipmentService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IEquipmentEngine equipmentEngine;

        public EquipmentService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            equipmentEngine = new EquipmentEngine(dbContext);
        }

        public int Save(EquipmentBO equipmentBO)
        {
            var equipment = new Equipment
            {
                EquipmentId = equipmentBO.EquipmentId,
                CustomsValue = equipmentBO.CustomsValue,
                FullyShipped = equipmentBO.FullyShipped,
                IsHardware = equipmentBO.IsHardware,
                LeftToShip = equipmentBO.LeftToShip,
                Priority = equipmentBO.Priority,
                ProjectId = equipmentBO.ProjectId,
                Quantity = equipmentBO.Quantity,
                ReadyToShip = equipmentBO.ReadyToShip,
                ReleaseDate = equipmentBO.ReleaseDate,
                SalePrice = equipmentBO.SalePrice,
                ShippedQuantity = equipmentBO.ShippedQuantity,
                TotalWeight = equipmentBO.TotalWeight,
                TotalWeightShipped = equipmentBO.TotalWeightShipped,
                UnitWeight = equipmentBO.UnitWeight,
                CountryOfOrigin = (equipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (equipmentBO.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (equipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (equipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (equipmentBO.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (equipmentBO.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (equipmentBO.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (equipmentBO.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (equipmentBO.WorkOrderNumber ?? string.Empty).ToUpperInvariant()
            };

            equipmentEngine.AddNewEquipment(equipment);

            dbContext.SaveChanges();

            //this will dispose and reinstantiate a new context so we get the latest updates
            //needed for ajax call of create equipment not getting the newest data from trigger update
            equipmentEngine.SetDBContext(new WendtEquipmentTrackingEntities());

            return equipment.EquipmentId;
        }

        public void SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {

            var equipments = equipmentBOs.Select(x => new Equipment
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = (x.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (x.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (x.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (x.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (x.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (x.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (x.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (x.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (x.WorkOrderNumber ?? string.Empty).ToUpperInvariant()
            });


            equipmentEngine.AddAllNewEquipment(equipments);

            dbContext.SaveChanges();
        }

        public void Update(EquipmentBO alteredEquipmentBO)
        {
            var oldEquipment = equipmentEngine.Get(EquipmentSpecs.Id(alteredEquipmentBO.EquipmentId));

            //oldEquipment.EquipmentId = alteredEquipmentBO.EquipmentId;
            oldEquipment.CustomsValue = alteredEquipmentBO.CustomsValue;
            //oldEquipment.FullyShipped = alteredEquipmentBO.FullyShipped;
            oldEquipment.IsHardware = alteredEquipmentBO.IsHardware;
            //oldEquipment.LeftToShip = alteredEquipmentBO.LeftToShip;
            oldEquipment.Priority = alteredEquipmentBO.Priority;
            //oldEquipment.ProjectId = alteredEquipmentBO.ProjectId;
            oldEquipment.Quantity = alteredEquipmentBO.Quantity;
            oldEquipment.ReadyToShip = alteredEquipmentBO.ReadyToShip;
            oldEquipment.ReleaseDate = alteredEquipmentBO.ReleaseDate;
            oldEquipment.SalePrice = alteredEquipmentBO.SalePrice;
            //oldEquipment.ShippedQuantity = alteredEquipmentBO.ShippedQuantity;
            //oldEquipment.TotalWeight = alteredEquipmentBO.TotalWeight;
            //oldEquipment.TotalWeightShipped = alteredEquipmentBO.TotalWeightShipped;
            oldEquipment.UnitWeight = alteredEquipmentBO.UnitWeight;
            oldEquipment.CountryOfOrigin = (alteredEquipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
            oldEquipment.Description = (alteredEquipmentBO.Description ?? string.Empty).ToUpperInvariant();
            oldEquipment.DrawingNumber = (alteredEquipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant();
            oldEquipment.EquipmentName = (alteredEquipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant();
            oldEquipment.HTSCode = (alteredEquipmentBO.HTSCode ?? string.Empty).ToUpperInvariant();
            oldEquipment.Notes = (alteredEquipmentBO.Notes ?? string.Empty).ToUpperInvariant();
            oldEquipment.ShippedFrom = (alteredEquipmentBO.ShippedFrom ?? string.Empty).ToUpperInvariant();
            oldEquipment.ShippingTagNumber = (alteredEquipmentBO.ShippingTagNumber ?? string.Empty).ToUpperInvariant();
            oldEquipment.WorkOrderNumber = (alteredEquipmentBO.WorkOrderNumber ?? string.Empty).ToUpperInvariant();

            equipmentEngine.UpdateEquipment(oldEquipment);

            dbContext.SaveChanges();

            //this will dispose and reinstantiate a new context so we get the latest updates
            //needed for ajax call of edit equipment not getting the newest data from trigger update
            equipmentEngine.SetDBContext(new WendtEquipmentTrackingEntities());
        }

        public void UpdateAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            //Performance Issue?
            var oldEquipments = equipmentEngine.List(EquipmentSpecs.Ids(equipmentBOs.Select(e => e.EquipmentId)));

            foreach (var oldEquipment in oldEquipments)
            {
                var equipmentBO = equipmentBOs.SingleOrDefault(e => e.EquipmentId == oldEquipment.EquipmentId);

                if (equipmentBO != null)
                {
                    //oldEquipment.EquipmentId = equipmentBO.EquipmentId;
                    oldEquipment.CustomsValue = equipmentBO.CustomsValue;
                    //oldEquipment.FullyShipped = equipmentBO.FullyShipped;
                    oldEquipment.IsHardware = equipmentBO.IsHardware;
                    //oldEquipment.LeftToShip = equipmentBO.LeftToShip;
                    oldEquipment.Priority = equipmentBO.Priority;
                    //oldEquipment.ProjectId = equipmentBO.ProjectId;
                    oldEquipment.Quantity = equipmentBO.Quantity;
                    oldEquipment.ReadyToShip = equipmentBO.ReadyToShip;
                    oldEquipment.ReleaseDate = equipmentBO.ReleaseDate;
                    oldEquipment.SalePrice = equipmentBO.SalePrice;
                    //oldEquipment.ShippedQuantity = equipmentBO.ShippedQuantity;
                    //oldEquipment.TotalWeight = equipmentBO.TotalWeight;
                    //oldEquipment.TotalWeightShipped = equipmentBO.TotalWeightShipped;
                    oldEquipment.UnitWeight = equipmentBO.UnitWeight;
                    oldEquipment.CountryOfOrigin = (equipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
                    oldEquipment.Description = (equipmentBO.Description ?? string.Empty).ToUpperInvariant();
                    oldEquipment.DrawingNumber = (equipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant();
                    oldEquipment.EquipmentName = (equipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant();
                    oldEquipment.HTSCode = (equipmentBO.HTSCode ?? string.Empty).ToUpperInvariant();
                    oldEquipment.Notes = (equipmentBO.Notes ?? string.Empty).ToUpperInvariant();
                    oldEquipment.ShippedFrom = (equipmentBO.ShippedFrom ?? string.Empty).ToUpperInvariant();
                    oldEquipment.ShippingTagNumber = (equipmentBO.ShippingTagNumber ?? string.Empty).ToUpperInvariant();
                    oldEquipment.WorkOrderNumber = (equipmentBO.WorkOrderNumber ?? string.Empty).ToUpperInvariant();
                }
            }

            equipmentEngine.UpdateEquipments(oldEquipments);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var equipment = equipmentEngine.Get(EquipmentSpecs.Id(id));

            if (equipment != null)
            {
                equipmentEngine.DeleteEquipment(equipment);
            }

            dbContext.SaveChanges();
        }

        public IEnumerable<EquipmentBO> GetAll(int projectId)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority.HasValue ? x.Priority.Value : 0,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = x.CountryOfOrigin,
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                HTSCode = x.HTSCode,
                Notes = x.Notes,
                ShippedFrom = x.ShippedFrom,
                ShippingTagNumber = x.ShippingTagNumber,
                WorkOrderNumber = x.WorkOrderNumber,
                HasBillOfLading = x.BillOfLadingEquipments.Where(b => b.BillOfLading.IsCurrentRevision).Count() > 0,
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });


            return equipmentBOs.ToList();
        }

        public EquipmentBO GetById(int id)
        {
            var equipment = equipmentEngine.Get(EquipmentSpecs.Id(id));

            var equipmentBO = new EquipmentBO
            {
                EquipmentId = equipment.EquipmentId,
                CustomsValue = equipment.CustomsValue,
                FullyShipped = equipment.FullyShipped,
                IsHardware = equipment.IsHardware,
                LeftToShip = equipment.LeftToShip,
                Priority = equipment.Priority.HasValue ? equipment.Priority.Value : 0,
                ProjectId = equipment.ProjectId,
                Quantity = equipment.Quantity,
                ReadyToShip = equipment.ReadyToShip,
                ReleaseDate = equipment.ReleaseDate,
                SalePrice = equipment.SalePrice,
                ShippedQuantity = equipment.ShippedQuantity,
                TotalWeight = equipment.TotalWeight,
                TotalWeightShipped = equipment.TotalWeightShipped,
                UnitWeight = equipment.UnitWeight,
                CountryOfOrigin = (equipment.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (equipment.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (equipment.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (equipment.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (equipment.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (equipment.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (equipment.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (equipment.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (equipment.WorkOrderNumber ?? string.Empty).ToUpperInvariant()
            };

            return equipmentBO;
        }

        public IEnumerable<EquipmentBO> GetByBillOfLadingId(int billOfLadingId)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.BillOfLadingId(billOfLadingId));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority.HasValue ? x.Priority.Value : 0,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = x.CountryOfOrigin,
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                HTSCode = x.HTSCode,
                Notes = x.Notes,
                ShippedFrom = x.ShippedFrom,
                ShippingTagNumber = x.ShippingTagNumber,
                WorkOrderNumber = x.WorkOrderNumber,

                HasBillOfLading = x.BillOfLadingEquipments.Where(b => b.BillOfLading.IsCurrentRevision).Count() > 0,
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });

            return equipmentBOs.ToList();
        }

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shippingTagNumber, string description)
        {
            return GetHardwareByShippingTagNumberAndDescription(projectId, shippingTagNumber, description, null);
        }

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shippingTagNumber, string description, int? hardwareKitId)
        {
            var hardwareKitSpec = !EquipmentSpecs.IsAttachedToHardwareKit();
            if (hardwareKitId.HasValue)
            {
                hardwareKitSpec = hardwareKitSpec || EquipmentSpecs.HardwareKitId(hardwareKitId.Value);
            }


            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId) && EquipmentSpecs.IsHardware() && hardwareKitSpec && EquipmentSpecs.ShippingTagNumber(shippingTagNumber) && EquipmentSpecs.Description(description));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority.HasValue ? x.Priority.Value : 0,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = x.CountryOfOrigin,
                Description = x.Description,
                DrawingNumber = x.DrawingNumber,
                EquipmentName = x.EquipmentName,
                HTSCode = x.HTSCode,
                Notes = x.Notes,
                ShippedFrom = x.ShippedFrom,
                ShippingTagNumber = x.ShippingTagNumber,
                WorkOrderNumber = x.WorkOrderNumber,

                HasBillOfLading = x.BillOfLadingEquipments.Any(b => b.BillOfLading.IsCurrentRevision),
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });

            return equipmentBOs.ToList();
        }
    }
}
