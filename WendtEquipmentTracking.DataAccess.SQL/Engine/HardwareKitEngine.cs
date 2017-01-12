using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class HardwareKitEngine : IHardwareKitEngine
    {
        private IRepository<HardwareKit> repository = null;

        public HardwareKitEngine()
        {
            this.repository = new Repository<HardwareKit>();
        }

        public HardwareKitEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<HardwareKit>(dbContext);
        }

        public HardwareKitEngine(Repository<HardwareKit> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<HardwareKit> ListAll()
        {
            return this.repository.Find(!HardwareKitSpecs.IsDeleted())
                .Include(x => x.HardwareKitEquipments);
        }

        public HardwareKit Get(Specification<HardwareKit> specification)
        {
            return this.repository.Single(!HardwareKitSpecs.IsDeleted() && specification);
        }

        public IEnumerable<HardwareKit> List(Specification<HardwareKit> specification)
        {
            return this.repository.Find(!HardwareKitSpecs.IsDeleted() && specification)
                .Include(x => x.HardwareKitEquipments);
        }

        public int AddNewHardwareKit(HardwareKit hardwareKit)
        {
            var now = DateTime.Now;

            hardwareKit.CreatedDate = now;
            hardwareKit.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            hardwareKit.ModifiedDate = now;
            hardwareKit.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            hardwareKit.Revision = 1;
            hardwareKit.IsCurrentRevision = true;

            this.repository.Insert(hardwareKit);
            this.repository.Save();

            return hardwareKit.HardwareKitId;
        }

        public void UpdateHardwareKit(HardwareKit hardwareKit)
        {
            var now = DateTime.Now;

            hardwareKit.CreatedDate = now;
            hardwareKit.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            hardwareKit.ModifiedDate = now;
            hardwareKit.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            var currentHardwareKit = this.Get(HardwareKitSpecs.CurrentRevision() && HardwareKitSpecs.HardwareKitNumber(hardwareKit.HardwareKitNumber));
            currentHardwareKit.IsCurrentRevision = false;

            hardwareKit.Revision = currentHardwareKit.Revision + 1;
            hardwareKit.IsCurrentRevision = true;

            this.repository.Insert(hardwareKit);
            this.repository.Update(currentHardwareKit);
            this.repository.Save();
        }

        public void DeleteHardwareKit(HardwareKit hardwareKit)
        {
            hardwareKit.IsDeleted = true;
            hardwareKit.HardwareKitEquipments.ToList().ForEach(ble => ble.IsDeleted = true);
            hardwareKit.Equipments.ToList().ForEach(ble => ble.IsDeleted = true);

            this.repository.Update(hardwareKit);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<HardwareKit>(dbContext);
        }
    }
}
