
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
    public class HardwareCommercialCodeService : IHardwareCommercialCodeService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IHardwareCommercialCodeEngine hardwareCommercialCodeEngine;

        public HardwareCommercialCodeService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            hardwareCommercialCodeEngine = new HardwareCommercialCodeEngine(dbContext);
        }

        public void Save(HardwareCommercialCodeBO hardwareCommercialCodeBO)
        {
            var hardwareCommercialCode = new HardwareCommercialCode
            {
                CommodityCode = hardwareCommercialCodeBO.CommodityCode,
                Description = hardwareCommercialCodeBO.Description,
                HardwareCommercialCodeId = hardwareCommercialCodeBO.HardwareCommercialCodeId,
                PartNumber = hardwareCommercialCodeBO.PartNumber
            };

            hardwareCommercialCodeEngine.AddNewHardwareCommercialCode(hardwareCommercialCode);

            dbContext.SaveChanges();
        }

        public void SaveAll(IEnumerable<HardwareCommercialCodeBO> hardwareCommercialCodeBOs)
        {
            var hardwareCommercialCodes = hardwareCommercialCodeBOs.Select(x => new HardwareCommercialCode
            {
                CommodityCode = x.CommodityCode,
                Description = x.Description,
                PartNumber = x.PartNumber
            });

            hardwareCommercialCodeEngine.AddAllNewHardwareCommercialCode(hardwareCommercialCodes);

            dbContext.SaveChanges();
        }

        public IEnumerable<HardwareCommercialCodeBO> GetAll()
        {
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll();

            var hardwareCommercialCodeBOs = hardwareCommercialCodes.Select(x => new HardwareCommercialCodeBO
            {
                CommodityCode = x.CommodityCode,
                Description = x.Description,
                HardwareCommercialCodeId = x.HardwareCommercialCodeId,
                PartNumber = x.PartNumber
            });

            return hardwareCommercialCodeBOs.ToList();
        }

        public HardwareCommercialCodeBO GetById(int id)
        {
            var hardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(id));

            var hardwareCommercialCodeBO = new HardwareCommercialCodeBO
            {
                CommodityCode = hardwareCommercialCode.CommodityCode,
                Description = hardwareCommercialCode.Description,
                HardwareCommercialCodeId = hardwareCommercialCode.HardwareCommercialCodeId,
                PartNumber = hardwareCommercialCode.PartNumber
            };

            return hardwareCommercialCodeBO;
        }

        public void Update(HardwareCommercialCodeBO hardwareCommercialCodeBO)
        {
            var oldHardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(hardwareCommercialCodeBO.HardwareCommercialCodeId));

            oldHardwareCommercialCode.CommodityCode = hardwareCommercialCodeBO.CommodityCode;
            oldHardwareCommercialCode.Description = hardwareCommercialCodeBO.Description;
            oldHardwareCommercialCode.HardwareCommercialCodeId = hardwareCommercialCodeBO.HardwareCommercialCodeId;
            oldHardwareCommercialCode.PartNumber = hardwareCommercialCodeBO.PartNumber;

            hardwareCommercialCodeEngine.UpdateHardwareCommercialCode(oldHardwareCommercialCode);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var hardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(id));

            if (hardwareCommercialCode != null)
            {
                hardwareCommercialCodeEngine.DeleteHardwareCommercialCode(hardwareCommercialCode);
            }

            dbContext.SaveChanges();
        }
    }
}
