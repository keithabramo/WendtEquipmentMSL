
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
    public class TruckingScheduleService : ITruckingScheduleService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private ITruckingScheduleEngine truckingScheduleEngine;

        public TruckingScheduleService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            truckingScheduleEngine = new TruckingScheduleEngine(dbContext);
        }

        public IEnumerable<int> SaveAll(IEnumerable<TruckingScheduleBO> truckingScheduleBOs)
        {
            var truckingSchedules = truckingScheduleBOs.Select(x => new TruckingSchedule
            {
                Carrier = x.Carrier,
                Comments = x.Comments,
                Description = x.Description,
                Dimensions = x.Dimensions,
                NumPieces = x.NumPieces,
                PickUpDate = x.PickUpDate,
                ProjectId = x.ProjectId,
                PurchaseOrder = x.PurchaseOrder,
                ShipFromVendorId = x.ShipFromVendorId,
                ShipToVendorId = x.ShipToVendorId,
                Status = x.Status,
                TruckingScheduleId = x.TruckingScheduleId,
                Weight = x.Weight,
                WorkOrder = x.WorkOrder,
                RequestDate = x.RequestDate,
                RequestedBy = x.RequestedBy
            }).ToList();

            truckingScheduleEngine.AddNewTruckingSchedules(truckingSchedules);

            dbContext.SaveChanges();

            return truckingSchedules.Select(x => x.TruckingScheduleId).ToList();
        }

        public IEnumerable<TruckingScheduleBO> GetAll()
        {
            var truckingSchedules = truckingScheduleEngine.ListAll();

            var truckingScheduleBOs = truckingSchedules.Select(x => new TruckingScheduleBO
            {
                Carrier = x.Carrier,
                Comments = x.Comments,
                Description = x.Description,
                Dimensions = x.Dimensions,
                NumPieces = x.NumPieces,
                PickUpDate = x.PickUpDate,
                ProjectId = x.ProjectId,
                PurchaseOrder = x.PurchaseOrder,
                ShipFromVendorId = x.ShipFromVendorId,
                ShipToVendorId = x.ShipToVendorId,
                Status = x.Status,
                TruckingScheduleId = x.TruckingScheduleId,
                Weight = x.Weight,
                WorkOrder = x.WorkOrder,
                RequestDate = x.RequestDate,
                RequestedBy = x.RequestedBy,
                Project = x.Project != null ? new ProjectBO
                {
                    ProjectId = x.Project.ProjectId,
                    ProjectNumber = x.Project.ProjectNumber
                } : null,
                ShipFromVendor = x.Vendor != null ? new VendorBO
                {
                    VendorId = x.Vendor.VendorId,
                    Name = x.Vendor.Name,
                    Address = x.Vendor.Address,
                    Contact1 = x.Vendor.Contact1,
                    Email = x.Vendor.Email,
                    PhoneFax = x.Vendor.PhoneFax
                } : null,
                ShipToVendor = x.Vendor1 != null ? new VendorBO
                {
                    VendorId = x.Vendor1.VendorId,
                    Name = x.Vendor1.Name,
                    Address = x.Vendor1.Address,
                    Contact1 = x.Vendor1.Contact1,
                    Email = x.Vendor1.Email,
                    PhoneFax = x.Vendor1.PhoneFax
                } : null
            });

            return truckingScheduleBOs.ToList();
        }

        public IEnumerable<TruckingScheduleBO> GetByIds(IEnumerable<int> ids)
        {
            var truckingSchedules = truckingScheduleEngine.List(TruckingScheduleSpecs.Ids(ids));

            var truckingScheduleBOs = truckingSchedules.Select(x => new TruckingScheduleBO
            {
                Carrier = x.Carrier,
                Comments = x.Comments,
                Description = x.Description,
                Dimensions = x.Dimensions,
                NumPieces = x.NumPieces,
                PickUpDate = x.PickUpDate,
                ProjectId = x.ProjectId,
                PurchaseOrder = x.PurchaseOrder,
                ShipFromVendorId = x.ShipFromVendorId,
                ShipToVendorId = x.ShipToVendorId,
                Status = x.Status,
                TruckingScheduleId = x.TruckingScheduleId,
                Weight = x.Weight,
                WorkOrder = x.WorkOrder,
                RequestDate = x.RequestDate,
                RequestedBy = x.RequestedBy,
                Project = x.Project != null ? new ProjectBO
                {
                    ProjectId = x.Project.ProjectId,
                    ProjectNumber = x.Project.ProjectNumber
                } : null,
                ShipFromVendor = x.Vendor != null ? new VendorBO
                {
                    VendorId = x.Vendor.VendorId,
                    Name = x.Vendor.Name,
                    Address = x.Vendor.Address,
                    Contact1 = x.Vendor.Contact1,
                    Email = x.Vendor.Email,
                    PhoneFax = x.Vendor.PhoneFax
                } : null,
                ShipToVendor = x.Vendor1 != null ? new VendorBO
                {
                    VendorId = x.Vendor1.VendorId,
                    Name = x.Vendor1.Name,
                    Address = x.Vendor1.Address,
                    Contact1 = x.Vendor1.Contact1,
                    Email = x.Vendor1.Email,
                    PhoneFax = x.Vendor1.PhoneFax
                } : null
            });

            return truckingScheduleBOs.ToList();
        }

        public void Update(TruckingScheduleBO truckingScheduleBO)
        {
            var oldTruckingSchedule = truckingScheduleEngine.Get(TruckingScheduleSpecs.Id(truckingScheduleBO.TruckingScheduleId));
            oldTruckingSchedule.Carrier = truckingScheduleBO.Carrier;
            oldTruckingSchedule.Comments = truckingScheduleBO.Comments;
            oldTruckingSchedule.Description = truckingScheduleBO.Description;
            oldTruckingSchedule.Dimensions = truckingScheduleBO.Dimensions;
            oldTruckingSchedule.NumPieces = truckingScheduleBO.NumPieces;
            oldTruckingSchedule.PickUpDate = truckingScheduleBO.PickUpDate;
            oldTruckingSchedule.ProjectId = truckingScheduleBO.ProjectId;
            oldTruckingSchedule.PurchaseOrder = truckingScheduleBO.PurchaseOrder;
            oldTruckingSchedule.ShipFromVendorId = truckingScheduleBO.ShipFromVendorId;
            oldTruckingSchedule.ShipToVendorId = truckingScheduleBO.ShipToVendorId;
            oldTruckingSchedule.Status = truckingScheduleBO.Status;
            oldTruckingSchedule.TruckingScheduleId = truckingScheduleBO.TruckingScheduleId;
            oldTruckingSchedule.Weight = truckingScheduleBO.Weight;
            oldTruckingSchedule.WorkOrder = truckingScheduleBO.WorkOrder;
            oldTruckingSchedule.RequestDate = truckingScheduleBO.RequestDate;
            oldTruckingSchedule.RequestedBy = truckingScheduleBO.RequestedBy;

            truckingScheduleEngine.UpdateTruckingSchedule(oldTruckingSchedule);

            dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<TruckingScheduleBO> truckingScheduleBOs)
        {
            //Performance Issue?
            var oldTruckingSchedules = truckingScheduleEngine.List(TruckingScheduleSpecs.Ids(truckingScheduleBOs.Select(x => x.TruckingScheduleId))).ToList();

            foreach (var oldTruckingSchedule in oldTruckingSchedules)
            {
                var truckingScheduleBO = truckingScheduleBOs.FirstOrDefault(x => x.TruckingScheduleId == oldTruckingSchedule.TruckingScheduleId);

                if (truckingScheduleBO != null)
                {
                    oldTruckingSchedule.Carrier = truckingScheduleBO.Carrier;
                    oldTruckingSchedule.Comments = truckingScheduleBO.Comments;
                    oldTruckingSchedule.Description = truckingScheduleBO.Description;
                    oldTruckingSchedule.Dimensions = truckingScheduleBO.Dimensions;
                    oldTruckingSchedule.NumPieces = truckingScheduleBO.NumPieces;
                    oldTruckingSchedule.PickUpDate = truckingScheduleBO.PickUpDate;
                    oldTruckingSchedule.ProjectId = truckingScheduleBO.ProjectId;
                    oldTruckingSchedule.PurchaseOrder = truckingScheduleBO.PurchaseOrder;
                    oldTruckingSchedule.ShipFromVendorId = truckingScheduleBO.ShipFromVendorId;
                    oldTruckingSchedule.ShipToVendorId = truckingScheduleBO.ShipToVendorId;
                    oldTruckingSchedule.Status = truckingScheduleBO.Status;
                    oldTruckingSchedule.TruckingScheduleId = truckingScheduleBO.TruckingScheduleId;
                    oldTruckingSchedule.Weight = truckingScheduleBO.Weight;
                    oldTruckingSchedule.WorkOrder = truckingScheduleBO.WorkOrder;
                    oldTruckingSchedule.RequestDate = truckingScheduleBO.RequestDate;
                    oldTruckingSchedule.RequestedBy = truckingScheduleBO.RequestedBy;
                }
            }

            truckingScheduleEngine.UpdateTruckingSchedules(oldTruckingSchedules);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var truckingSchedule = truckingScheduleEngine.Get(TruckingScheduleSpecs.Id(id));

            if (truckingSchedule != null)
            {
                truckingScheduleEngine.DeleteTruckingSchedule(truckingSchedule);
            }

            dbContext.SaveChanges();
        }
    }
}
