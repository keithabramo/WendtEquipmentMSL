using System;
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

        public IEnumerable<int> SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {

            var equipments = equipmentBOs.Select(x => new Equipment
            {
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                PriorityId = x.PriorityId,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.IsHardware ? .01 : x.UnitWeight.HasValue ? (double?)Math.Round(x.UnitWeight.Value, 2, MidpointRounding.AwayFromZero) : null,
                CountryOfOrigin = (x.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (x.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (x.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (x.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (x.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (x.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (x.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (x.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (x.WorkOrderNumber ?? string.Empty).ToUpperInvariant()
            }).ToList();


            equipmentEngine.AddAllNewEquipment(equipments);

            dbContext.SaveChanges();

            return equipments.Select(x => x.EquipmentId).ToList();
        }

        public void UpdateAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            //Performance Issue?
            var oldEquipments = equipmentEngine.List(EquipmentSpecs.Ids(equipmentBOs.Select(e => e.EquipmentId))).ToList();

            foreach (var oldEquipment in oldEquipments)
            {
                var equipmentBO = equipmentBOs.SingleOrDefault(e => e.EquipmentId == oldEquipment.EquipmentId);

                if (equipmentBO != null)
                {
                    //NOTE: we don't update the quantity or equipment name of a hardware kit record so we have code to check for that

                    //oldEquipment.EquipmentId = equipmentBO.EquipmentId;
                    oldEquipment.CustomsValue = equipmentBO.CustomsValue;
                    //oldEquipment.FullyShipped = equipmentBO.FullyShipped;
                    oldEquipment.IsHardware = equipmentBO.IsHardware;
                    //oldEquipment.LeftToShip = equipmentBO.LeftToShip;
                    oldEquipment.PriorityId = equipmentBO.PriorityId;
                    //oldEquipment.ProjectId = equipmentBO.ProjectId;
                    oldEquipment.Quantity = equipmentBO.IsHardwareKit ? oldEquipment.Quantity : equipmentBO.Quantity;
                    oldEquipment.ReadyToShip = equipmentBO.ReadyToShip;
                    oldEquipment.ReleaseDate = equipmentBO.ReleaseDate;
                    oldEquipment.SalePrice = equipmentBO.SalePrice;
                    //oldEquipment.ShippedQuantity = equipmentBO.ShippedQuantity;
                    //oldEquipment.TotalWeight = equipmentBO.TotalWeight;
                    //oldEquipment.TotalWeightShipped = equipmentBO.TotalWeightShipped;
                    oldEquipment.UnitWeight = equipmentBO.IsHardware ? .01 : equipmentBO.UnitWeight.HasValue ? (double?)Math.Round(equipmentBO.UnitWeight.Value, 2, MidpointRounding.AwayFromZero) : null;
                    oldEquipment.CountryOfOrigin = (equipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
                    oldEquipment.Description = (equipmentBO.Description ?? string.Empty).ToUpperInvariant();
                    oldEquipment.DrawingNumber = (equipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant();
                    oldEquipment.EquipmentName = equipmentBO.IsHardwareKit ? oldEquipment.EquipmentName : (equipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant();
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

        public void DeleteAll(IEnumerable<int> ids)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.Ids(ids)).ToList();

            if (equipments != null)
            {
                equipmentEngine.DeleteEquipments(equipments);
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
                PriorityId = x.Priority != null && !x.Priority.IsDeleted ? x.PriorityId : null,
                Priority = x.Priority != null && !x.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = x.Priority.PriorityId,
                    PriorityNumber = x.Priority.PriorityNumber
                } : null,
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
                BillOfLadingEquipments = x.BillOfLadingEquipments
                    .Where(y => !y.IsDeleted && y.BillOfLading.IsCurrentRevision)
                    .Select(y => new BillOfLadingEquipmentBO
                    {
                        BillOfLading = new BillOfLadingBO
                        {
                            BillOfLadingId = y.BillOfLading.BillOfLadingId,
                            BillOfLadingNumber = y.BillOfLading.BillOfLadingNumber,
                            DateShipped = y.BillOfLading.DateShipped
                        }
                    }).ToList(),
                HasBillOfLading = x.BillOfLadingEquipments.Any(b => !b.IsDeleted && b.BillOfLading.IsCurrentRevision),
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => !be.IsDeleted && be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            }).ToList();


            return equipmentBOs.ToList();
        }

        public IEnumerable<EquipmentBO> GetForDuplicateCheck(int projectId)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                PriorityId = x.Priority != null && !x.Priority.IsDeleted ? x.PriorityId : null,
                Priority = x.Priority != null && !x.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = x.Priority.PriorityId,
                    PriorityNumber = x.Priority.PriorityNumber
                } : null,
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
                WorkOrderNumber = x.WorkOrderNumber
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
                PriorityId = equipment.Priority != null && !equipment.Priority.IsDeleted ? equipment.PriorityId : null,
                Priority = equipment.Priority != null && !equipment.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = equipment.Priority.PriorityId,
                    PriorityNumber = equipment.Priority.PriorityNumber
                } : null,
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
                WorkOrderNumber = (equipment.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),
                BillOfLadingEquipments = equipment.BillOfLadingEquipments.Where(x => !x.IsDeleted && x.BillOfLading.IsCurrentRevision).Select(x => new BillOfLadingEquipmentBO
                {
                    BillOfLadingEquipmentId = x.BillOfLadingEquipmentId,
                    BillOfLadingId = x.BillOfLadingId,
                    EquipmentId = x.EquipmentId,
                    Quantity = x.Quantity,
                    ShippedFrom = x.ShippedFrom,
                    BillOfLading = new BillOfLadingBO
                    {
                        BillOfLadingId = x.BillOfLading.BillOfLadingId,
                        BillOfLadingNumber = x.BillOfLading.BillOfLadingNumber,
                        Carrier = x.BillOfLading.Carrier,
                        DateShipped = x.BillOfLading.DateShipped,
                        FreightTerms = x.BillOfLading.FreightTerms,
                        IsCurrentRevision = x.BillOfLading.IsCurrentRevision,
                        ProjectId = x.BillOfLading.ProjectId,
                        Revision = x.BillOfLading.Revision,
                        ToStorage = x.BillOfLading.ToStorage,
                        TrailerNumber = x.BillOfLading.TrailerNumber,
                        ShippedFrom = x.BillOfLading.ShippedFrom,
                        ShippedTo = x.BillOfLading.ShippedTo,
                    },
                }).ToList()
            };

            return equipmentBO;
        }

        public IEnumerable<EquipmentBO> GetByIds(IEnumerable<int> ids)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.Ids(ids));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                PriorityId = x.Priority != null && !x.Priority.IsDeleted ? x.PriorityId : null,
                Priority = x.Priority != null && !x.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = x.Priority.PriorityId,
                    PriorityNumber = x.Priority.PriorityNumber
                } : null,
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
                BillOfLadingEquipments = x.BillOfLadingEquipments
                    .Where(y => !y.IsDeleted && y.BillOfLading.IsCurrentRevision)
                    .Select(y => new BillOfLadingEquipmentBO
                    {
                        BillOfLading = new BillOfLadingBO
                        {
                            BillOfLadingId = y.BillOfLading.BillOfLadingId,
                            BillOfLadingNumber = y.BillOfLading.BillOfLadingNumber,
                            DateShipped = y.BillOfLading.DateShipped
                        }
                    }).ToList(),
                HasBillOfLading = x.BillOfLadingEquipments.Any(b => !b.IsDeleted && b.BillOfLading.IsCurrentRevision),
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => !be.IsDeleted && be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });

            return equipmentBOs.ToList();
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
                PriorityId = x.Priority != null && !x.Priority.IsDeleted ? x.PriorityId : null,
                Priority = x.Priority != null && !x.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = x.Priority.PriorityId,
                    PriorityNumber = x.Priority.PriorityNumber
                } : null,
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

                HasBillOfLading = x.BillOfLadingEquipments.Any(b => !b.IsDeleted && b.BillOfLading.IsCurrentRevision),
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => !be.IsDeleted && be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });

            return equipmentBOs.ToList();
        }

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shippingTagNumber, string description)
        {
            return GetHardwareByShippingTagNumberAndDescription(projectId, shippingTagNumber, description, null);
        }

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shippingTagNumber, string description, int? hardwareKitId)
        {
            //Is either not attached to a current hardware kit or it is attached to the hardware kit of the id passed in
            var hardwareKitSpec = !EquipmentSpecs.IsAttachedToHardwareKit();
            if (hardwareKitId.HasValue)
            {
                hardwareKitSpec = hardwareKitSpec || EquipmentSpecs.IsAssociatedToHardwareKit(hardwareKitId.Value);
            }


            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId) && EquipmentSpecs.IsHardware() && hardwareKitSpec && EquipmentSpecs.ShippingTagNumber(shippingTagNumber) && EquipmentSpecs.Description(description));

            var equipmentBOs = equipments.Select(x => new EquipmentBO
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                IsHardware = x.IsHardware,
                LeftToShip = x.LeftToShip,
                PriorityId = x.Priority != null && !x.Priority.IsDeleted ? x.PriorityId : null,
                Priority = x.Priority != null && !x.Priority.IsDeleted ? new PriorityBO
                {
                    PriorityId = x.Priority.PriorityId,
                    PriorityNumber = x.Priority.PriorityNumber
                } : null,
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

                HasBillOfLading = x.BillOfLadingEquipments.Any(b => !b.BillOfLading.IsDeleted && b.BillOfLading.IsCurrentRevision),
                HasBillOfLadingInStorage = x.BillOfLadingEquipments.Any(be => !be.BillOfLading.IsDeleted && be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision),
                IsHardwareKit = x.HardwareKit != null,
                IsAssociatedToHardwareKit = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision),
                AssociatedHardwareKitNumber = x.HardwareKitEquipments.Any(h => h.HardwareKit.IsCurrentRevision) ? x.HardwareKitEquipments.Where(h => !h.IsDeleted && h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty
            });

            return equipmentBOs.ToList();
        }
    }
}
