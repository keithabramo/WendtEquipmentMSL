using AutoMapper;
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
        private IHardwareCommercialCodeEngine hardwareCommercialCodeEngine;

        public HardwareCommercialCodeService()
        {
            hardwareCommercialCodeEngine = new HardwareCommercialCodeEngine();
        }

        public void Save(HardwareCommercialCodeBO hardwareCommercialCodeBO)
        {
            var hardwareCommercialCode = Mapper.Map<HardwareCommercialCode>(hardwareCommercialCodeBO);

            hardwareCommercialCodeEngine.AddNewHardwareCommercialCode(hardwareCommercialCode);
        }

        public void SaveAll(IEnumerable<HardwareCommercialCodeBO> hardwareCommercialCodeBOs)
        {
            var hardwareCommercialCodes = Mapper.Map<IEnumerable<HardwareCommercialCode>>(hardwareCommercialCodeBOs);

            hardwareCommercialCodeEngine.AddAllNewHardwareCommercialCode(hardwareCommercialCodes);
        }

        public IEnumerable<HardwareCommercialCodeBO> GetAll()
        {
            var hardwareCommercialCodes = hardwareCommercialCodeEngine.ListAll().ToList();

            var hardwareCommercialCodeBOs = Mapper.Map<IEnumerable<HardwareCommercialCodeBO>>(hardwareCommercialCodes);

            return hardwareCommercialCodeBOs;
        }

        public HardwareCommercialCodeBO GetById(int id)
        {
            var hardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(id));

            var hardwareCommercialCodeBO = Mapper.Map<HardwareCommercialCodeBO>(hardwareCommercialCode);

            return hardwareCommercialCodeBO;
        }

        public void Update(HardwareCommercialCodeBO hardwareCommercialCodeBO)
        {
            var oldHardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(hardwareCommercialCodeBO.HardwareCommercialCodeId));

            Mapper.Map<HardwareCommercialCodeBO, HardwareCommercialCode>(hardwareCommercialCodeBO, oldHardwareCommercialCode);

            hardwareCommercialCodeEngine.UpdateHardwareCommercialCode(oldHardwareCommercialCode);
        }

        public void Delete(int id)
        {
            var hardwareCommercialCode = hardwareCommercialCodeEngine.Get(HardwareCommercialCodeSpecs.Id(id));

            if (hardwareCommercialCode != null)
            {
                hardwareCommercialCodeEngine.DeleteHardwareCommercialCode(hardwareCommercialCode);
            }
        }
    }
}
