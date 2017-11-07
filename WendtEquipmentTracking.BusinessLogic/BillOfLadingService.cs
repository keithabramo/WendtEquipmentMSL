
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
    public class BillOfLadingService : IBillOfLadingService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IBillOfLadingEngine billOfLadingEngine;
        private IEquipmentEngine equipmentEngine;


        public BillOfLadingService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            billOfLadingEngine = new BillOfLadingEngine(dbContext);
            equipmentEngine = new EquipmentEngine(dbContext);
        }

        public void Save(BillOfLadingBO billOfLadingBO)
        {

            var billOfLading = new BillOfLading
            {
                BillOfLadingId = billOfLadingBO.BillOfLadingId,
                BillOfLadingNumber = billOfLadingBO.BillOfLadingNumber,
                Carrier = billOfLadingBO.Carrier,
                DateShipped = billOfLadingBO.DateShipped,
                FreightTerms = billOfLadingBO.FreightTerms,
                IsCurrentRevision = billOfLadingBO.IsCurrentRevision,
                ProjectId = billOfLadingBO.ProjectId,
                Revision = billOfLadingBO.Revision,
                ToStorage = billOfLadingBO.ToStorage,
                TrailerNumber = billOfLadingBO.TrailerNumber,
                BillOfLadingEquipments = billOfLadingBO.BillOfLadingEquipments.Select(e => new BillOfLadingEquipment
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                }).ToList()
            };

            billOfLadingEngine.AddNewBillOfLading(billOfLading);

            foreach (var billOfLadingEquipment in billOfLadingBO.BillOfLadingEquipments)
            {
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(billOfLadingEquipment.EquipmentId));
                equipment.HTSCode = (billOfLadingEquipment.HTSCode ?? string.Empty).ToUpper();
                equipment.CountryOfOrigin = (billOfLadingEquipment.CountryOfOrigin ?? string.Empty).ToUpper();
                equipment.ReadyToShip = equipment.ReadyToShip - billOfLadingEquipment.Quantity;

                equipmentEngine.UpdateEquipment(equipment);
            }

            dbContext.SaveChanges();
        }

        public void Update(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = new BillOfLading
            {
                BillOfLadingId = billOfLadingBO.BillOfLadingId,
                BillOfLadingNumber = billOfLadingBO.BillOfLadingNumber,
                Carrier = billOfLadingBO.Carrier,
                DateShipped = billOfLadingBO.DateShipped,
                FreightTerms = billOfLadingBO.FreightTerms,
                IsCurrentRevision = billOfLadingBO.IsCurrentRevision,
                ProjectId = billOfLadingBO.ProjectId,
                Revision = billOfLadingBO.Revision,
                ToStorage = billOfLadingBO.ToStorage,
                TrailerNumber = billOfLadingBO.TrailerNumber,
                BillOfLadingEquipments = billOfLadingBO.BillOfLadingEquipments.Select(e => new BillOfLadingEquipment
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                }).ToList()
            };


            //update deleted and updated equipements
            var oldBillOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.ProjectId(billOfLadingBO.ProjectId) && BillOfLadingSpecs.BillOfLadingNumber(billOfLadingBO.BillOfLadingNumber) && BillOfLadingSpecs.CurrentRevision());
            foreach (var oldBOLE in oldBillOfLading.BillOfLadingEquipments)
            {
                var updatedBOLE = billOfLadingBO.BillOfLadingEquipments.FirstOrDefault(x => x.EquipmentId == oldBOLE.EquipmentId);
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(oldBOLE.EquipmentId));

                if (updatedBOLE != null)
                {
                    equipment.ReadyToShip = equipment.ReadyToShip - (updatedBOLE.Quantity - oldBOLE.Quantity);
                    equipment.HTSCode = (updatedBOLE.HTSCode ?? string.Empty).ToUpper();
                    equipment.CountryOfOrigin = (updatedBOLE.CountryOfOrigin ?? string.Empty).ToUpper();
                }
                else
                {
                    equipment.ReadyToShip = equipment.ReadyToShip + oldBOLE.Quantity;
                }


                equipmentEngine.UpdateEquipment(equipment);
            }

            //update new equipment for this bol
            var newEquipment = billOfLadingBO.BillOfLadingEquipments.Where(x => !oldBillOfLading.BillOfLadingEquipments.Any(old => old.EquipmentId == x.EquipmentId));
            foreach (var billOfLadingEquipment in newEquipment)
            {
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(billOfLadingEquipment.EquipmentId));

                equipment.ReadyToShip = equipment.ReadyToShip - billOfLadingEquipment.Quantity;
                equipment.HTSCode = (billOfLadingEquipment.HTSCode ?? string.Empty).ToUpper();
                equipment.CountryOfOrigin = (billOfLadingEquipment.CountryOfOrigin ?? string.Empty).ToUpper();

                equipmentEngine.UpdateEquipment(equipment);
            }


            billOfLadingEngine.UpdateBillOfLading(billOfLading);



            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            if (billOfLading != null)
            {
                billOfLadingEngine.DeleteBillOfLading(billOfLading);
            }

            dbContext.SaveChanges();
        }

        public IEnumerable<BillOfLadingBO> GetAll()
        {
            var billOfLadings = billOfLadingEngine.ListAll();

            var billOfLadingBOs = billOfLadings.Select(x => new BillOfLadingBO
            {
                BillOfLadingId = x.BillOfLadingId,
                BillOfLadingNumber = x.BillOfLadingNumber,
                Carrier = x.Carrier,
                DateShipped = x.DateShipped,
                FreightTerms = x.FreightTerms,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                ToStorage = x.ToStorage,
                TrailerNumber = x.TrailerNumber,
                BillOfLadingEquipments = x.BillOfLadingEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new BillOfLadingEquipmentBO
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                })

            });

            return billOfLadingBOs.ToList();
        }

        public BillOfLadingBO GetById(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            var billOfLadingBO = new BillOfLadingBO
            {
                BillOfLadingId = billOfLading.BillOfLadingId,
                BillOfLadingNumber = billOfLading.BillOfLadingNumber,
                Carrier = billOfLading.Carrier,
                DateShipped = billOfLading.DateShipped,
                FreightTerms = billOfLading.FreightTerms,
                IsCurrentRevision = billOfLading.IsCurrentRevision,
                ProjectId = billOfLading.ProjectId,
                Revision = billOfLading.Revision,
                ToStorage = billOfLading.ToStorage,
                TrailerNumber = billOfLading.TrailerNumber,
                BillOfLadingEquipments = billOfLading.BillOfLadingEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new BillOfLadingEquipmentBO
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                })
            };

            return billOfLadingBO;
        }

        public IEnumerable<BillOfLadingBO> GetByBillOfLadingNumber(int projectId, string billOfLadingNumber)
        {
            var billOfLadings = billOfLadingEngine.List(BillOfLadingSpecs.ProjectId(projectId) && BillOfLadingSpecs.BillOfLadingNumber(billOfLadingNumber));

            var billOfLadingBOs = billOfLadings.Select(x => new BillOfLadingBO
            {
                BillOfLadingId = x.BillOfLadingId,
                BillOfLadingNumber = x.BillOfLadingNumber,
                Carrier = x.Carrier,
                DateShipped = x.DateShipped,
                FreightTerms = x.FreightTerms,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                ToStorage = x.ToStorage,
                TrailerNumber = x.TrailerNumber,
                BillOfLadingEquipments = x.BillOfLadingEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new BillOfLadingEquipmentBO
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                })

            });

            return billOfLadingBOs.ToList();
        }
        public IEnumerable<BillOfLadingBO> GetCurrentByProject(int projectId)
        {
            var billOfLadings = billOfLadingEngine.List(BillOfLadingSpecs.ProjectId(projectId) && BillOfLadingSpecs.CurrentRevision());

            var billOfLadingBOs = billOfLadings.Select(x => new BillOfLadingBO
            {
                BillOfLadingId = x.BillOfLadingId,
                BillOfLadingNumber = x.BillOfLadingNumber,
                Carrier = x.Carrier,
                DateShipped = x.DateShipped,
                FreightTerms = x.FreightTerms,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                ToStorage = x.ToStorage,
                TrailerNumber = x.TrailerNumber,
                BillOfLadingEquipments = x.BillOfLadingEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new BillOfLadingEquipmentBO
                {
                    BillOfLadingEquipmentId = e.BillOfLadingEquipmentId,
                    BillOfLadingId = e.BillOfLadingId,
                    EquipmentId = e.EquipmentId,
                    Quantity = e.Quantity,
                    ShippedFrom = e.ShippedFrom
                })

            });

            return billOfLadingBOs.ToList();
        }

    }
}
