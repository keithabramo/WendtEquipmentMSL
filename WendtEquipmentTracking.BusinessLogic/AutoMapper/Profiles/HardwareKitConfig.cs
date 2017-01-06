using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtHardwareKitTracking.BusinessLogic.AutoMapper.Profiles
{
    public class HardwareKitConfig : Profile
    {
        public HardwareKitConfig()
        {

            base.CreateMap<HardwareKit, HardwareKitBO>();
            base.CreateMap<HardwareKitBO, HardwareKit>()
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());


        }
    }
}