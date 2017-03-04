using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class HardwareCommercialCodeConfig : Profile
    {
        public HardwareCommercialCodeConfig()
        {

            base.CreateMap<HardwareCommercialCode, HardwareCommercialCodeBO>();
            base.CreateMap<HardwareCommercialCodeBO, HardwareCommercialCode>();

            base.CreateMap<HardwareCommercialCode, HardwareCommercialCodeImport>();

        }
    }
}