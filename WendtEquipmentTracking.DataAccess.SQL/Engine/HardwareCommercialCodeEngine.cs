using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class HardwareCommercialCodeEngine : IHardwareCommercialCodeEngine
    {
        private IRepository<HardwareCommercialCode> repository = null;

        public HardwareCommercialCodeEngine()
        {
            this.repository = new Repository<HardwareCommercialCode>();
        }

        public HardwareCommercialCodeEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<HardwareCommercialCode>(dbContext);
        }

        public HardwareCommercialCodeEngine(Repository<HardwareCommercialCode> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<HardwareCommercialCode> ListAll()
        {
            return this.repository.Find(!HardwareCommercialCodeSpecs.IsDeleted());
        }

        public HardwareCommercialCode Get(Specification<HardwareCommercialCode> specification)
        {
            return this.repository.Single(!HardwareCommercialCodeSpecs.IsDeleted() && specification);
        }

        public IEnumerable<HardwareCommercialCode> List(Specification<HardwareCommercialCode> specification)
        {
            return this.repository.Find(!HardwareCommercialCodeSpecs.IsDeleted() && specification);
        }

        public void AddNewHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode)
        {
            var now = DateTime.Now;

            hardwareCommercialCode.CreatedDate = now;
            hardwareCommercialCode.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            hardwareCommercialCode.ModifiedDate = now;
            hardwareCommercialCode.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(hardwareCommercialCode);
            this.repository.Save();
        }

        public void AddAllNewHardwareCommercialCode(IEnumerable<HardwareCommercialCode> hardwareCommercialCodes)
        {
            var now = DateTime.Now;

            foreach (var hardwareCommercialCode in hardwareCommercialCodes)
            {
                hardwareCommercialCode.CreatedDate = now;
                hardwareCommercialCode.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                hardwareCommercialCode.ModifiedDate = now;
                hardwareCommercialCode.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            }

            this.repository.InsertAll(hardwareCommercialCodes);
            this.repository.Save();
        }

        public void UpdateHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode)
        {
            var now = DateTime.Now;

            hardwareCommercialCode.ModifiedDate = now;
            hardwareCommercialCode.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(hardwareCommercialCode);
            this.repository.Save();
        }

        public void DeleteHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode)
        {
            hardwareCommercialCode.IsDeleted = true;

            this.repository.Update(hardwareCommercialCode);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<HardwareCommercialCode>(dbContext);
        }
    }
}
