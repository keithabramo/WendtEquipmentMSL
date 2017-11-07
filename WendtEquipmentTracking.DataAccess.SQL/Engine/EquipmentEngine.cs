using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class EquipmentEngine : IEquipmentEngine
    {
        private IRepository<Equipment> repository = null;

        public EquipmentEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Equipment>(dbContext);
        }

        public EquipmentEngine(Repository<Equipment> repository)
        {
            this.repository = repository;
        }

        public IQueryable<Equipment> ListAll()
        {
            return this.repository.Find(!EquipmentSpecs.IsDeleted())
                .Include(x => x.BillOfLadingEquipments)
                .Include(x => x.BillOfLadingEquipments.Select(ble => ble.BillOfLading))
                .Include(x => x.HardwareKitEquipments)
                .Include(x => x.HardwareKitEquipments.Select(ble => ble.HardwareKit));
        }

        public Equipment Get(Specification<Equipment> specification)
        {
            return this.repository.Single(!EquipmentSpecs.IsDeleted() && specification);
        }

        public IQueryable<Equipment> List(Specification<Equipment> specification)
        {
            return this.repository.Find(!EquipmentSpecs.IsDeleted() && specification)
                .Include(x => x.BillOfLadingEquipments)
                .Include(x => x.BillOfLadingEquipments.Select(ble => ble.BillOfLading))
                .Include(x => x.HardwareKitEquipments)
                .Include(x => x.HardwareKitEquipments.Select(ble => ble.HardwareKit));
        }

        public void AddNewEquipment(Equipment equipment)
        {
            var now = DateTime.Now;

            equipment.CreatedDate = now;
            equipment.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            equipment.ModifiedDate = now;
            equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(equipment);
        }

        public void AddAllNewEquipment(IEnumerable<Equipment> equipments)
        {
            var now = DateTime.Now;

            foreach (var equipment in equipments)
            {
                equipment.CreatedDate = now;
                equipment.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                equipment.ModifiedDate = now;
                equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Insert(equipment);
            }

        }

        public void UpdateEquipment(Equipment equipment)
        {
            var now = DateTime.Now;

            equipment.ModifiedDate = now;
            equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            //TODO REMOVE!
            //throw new Exception("This is a test");


            this.repository.Update(equipment);

        }

        public void UpdateEquipments(IQueryable<Equipment> equipments)
        {
            var now = DateTime.Now;

            foreach (var equipment in equipments)
            {
                equipment.ModifiedDate = now;
                equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Update(equipment);
            }


        }

        public void DeleteEquipment(Equipment equipment)
        {
            equipment.IsDeleted = true;
            equipment.BillOfLadingEquipments.ToList().ForEach(ble => ble.IsDeleted = true);
            equipment.HardwareKitEquipments.ToList().ForEach(ble => ble.IsDeleted = true);

            this.repository.Update(equipment);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Equipment>(dbContext);
        }
    }
}
