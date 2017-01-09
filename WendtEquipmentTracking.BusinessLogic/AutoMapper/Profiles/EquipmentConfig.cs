using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class EquipmentConfig : Profile
    {
        public EquipmentConfig()
        {

            base.CreateMap<Equipment, EquipmentBO>();
            base.CreateMap<EquipmentBO, Equipment>()
                .ForMember(dest => dest.BillOfLadingEquipments, opt => opt.Ignore())
                .ForMember(dest => dest.HardwareKitEquipments, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<EquipmentRow, EquipmentBO>();

        }
    }
}