using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class HardwareCommercialCodeConfig : Profile
    {
        public HardwareCommercialCodeConfig()
        {

            base.CreateMap<HardwareCommercialCodeBO, HardwareCommercialCodeModel>();
            base.CreateMap<HardwareCommercialCodeModel, HardwareCommercialCodeBO>();

        }
    }
}