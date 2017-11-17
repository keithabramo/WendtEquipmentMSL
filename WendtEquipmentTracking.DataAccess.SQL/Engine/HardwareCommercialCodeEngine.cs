using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class HardwareCommercialCodeEngine : IHardwareCommercialCodeEngine
    {
        private IRepository<HardwareCommercialCode> repository = null;

        public HardwareCommercialCodeEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<HardwareCommercialCode>(dbContext);
        }

        public HardwareCommercialCodeEngine(Repository<HardwareCommercialCode> repository)
        {
            this.repository = repository;
        }

        public IQueryable<HardwareCommercialCode> ListAll()
        {
            return this.repository.Find(!HardwareCommercialCodeSpecs.IsDeleted());
        }

        public HardwareCommercialCode Get(Specification<HardwareCommercialCode> specification)
        {
            return this.repository.Single(!HardwareCommercialCodeSpecs.IsDeleted() && specification);
        }

        public IQueryable<HardwareCommercialCode> List(Specification<HardwareCommercialCode> specification)
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
                this.repository.Insert(hardwareCommercialCode);
            }
        }

        public void UpdateHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode)
        {
            var now = DateTime.Now;

            hardwareCommercialCode.ModifiedDate = now;
            hardwareCommercialCode.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(hardwareCommercialCode);

        }

        public void DeleteHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode)
        {
            var now = DateTime.Now;

            hardwareCommercialCode.ModifiedDate = now;
            hardwareCommercialCode.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            hardwareCommercialCode.IsDeleted = true;

            this.repository.Update(hardwareCommercialCode);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<HardwareCommercialCode>(dbContext);
        }
    }
}
