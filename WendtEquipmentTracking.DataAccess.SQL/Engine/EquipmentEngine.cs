using System;
using System.Collections.Generic;
using System.Data.Entity;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class EquipmentEngine : IEquipmentEngine
    {
        private IRepository<Equipment> repository = null;

        public EquipmentEngine() {
            this.repository = new Repository<Equipment>();
        }

        public EquipmentEngine(WendtEquipmentTrackingEntities dbContext) {
            this.repository = new Repository<Equipment>(dbContext);
        }

        public EquipmentEngine(Repository<Equipment> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Equipment> ListAll() {
            return this.repository.GetAll()
                .Include(x => x.BillOfLandings);
        }

        public Equipment Get(Specification<Equipment> specification) {
            return this.repository.Single(specification);
        }

        public IEnumerable<Equipment> List(Specification<Equipment> specification) {
            return this.repository.Find(specification)
                .Include(x => x.BillOfLandings);
        }

        public void AddNewEquipment(Equipment equipment) {
            var now = DateTime.Now;

            equipment.CreatedDate = now;
            equipment.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            equipment.ModifiedDate = now;
            equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            foreach(var billOfLanding in equipment.BillOfLandings)
            {
                billOfLanding.CreatedDate = equipment.CreatedDate;
                billOfLanding.CreatedBy = equipment.CreatedBy;
                billOfLanding.ModifiedDate = equipment.ModifiedDate;
                billOfLanding.ModifiedBy = equipment.ModifiedBy;
            }

            this.repository.Insert(equipment);
            this.repository.Save();
        }

        public void UpdateEquipment(Equipment equipment) {
            var now = DateTime.Now;

            equipment.ModifiedDate = now;
            equipment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            foreach (var billOfLanding in equipment.BillOfLandings)
            {
                if (billOfLanding.BillOfLandingId == 0)
                {
                    billOfLanding.CreatedDate = equipment.CreatedDate;
                    billOfLanding.CreatedBy = equipment.CreatedBy;
                }
                billOfLanding.ModifiedDate = equipment.ModifiedDate;
                billOfLanding.ModifiedBy = equipment.ModifiedBy;
            }

            this.repository.Update(equipment);
            this.repository.Save();
        }

        public void DeleteEquipment(Equipment equipment) {
            this.repository.Delete(equipment);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext) {
            this.repository = new Repository<Equipment>(dbContext);
        }
    }
}
