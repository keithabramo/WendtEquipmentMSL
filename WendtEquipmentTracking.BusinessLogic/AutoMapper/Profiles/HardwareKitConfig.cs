using AutoMapper;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtHardwareKitTracking.BusinessLogic.AutoMapper.Profiles
{
    public class HardwareKitConfig : Profile
    {
        public HardwareKitConfig()
        {

            base.CreateMap<HardwareKit, HardwareKitBO>()
                .ForMember(dest => dest.HardwareKitEquipments, opt => opt.MapFrom(src => src.HardwareKitEquipments.Where(o => !o.IsDeleted)));

            base.CreateMap<HardwareKitBO, HardwareKit>()
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<HardwareKitEquipmentBO, HardwareKitEquipment>().MaxDepth(1);
            base.CreateMap<HardwareKitEquipment, HardwareKitEquipmentBO>().MaxDepth(1);

            base.CreateMap<HardwareKitBO, Equipment>()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.HardwareKitNumber))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.HardwareKitEquipments.Sum(hke => hke.Quantity)));

        }
    }
}