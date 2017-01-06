using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class HardwareKitConfig : Profile
    {
        public HardwareKitConfig()
        {

            base.CreateMap<HardwareKitBO, HardwareKitModel>();
            base.CreateMap<HardwareKitModel, HardwareKitBO>();
        }
    }
}