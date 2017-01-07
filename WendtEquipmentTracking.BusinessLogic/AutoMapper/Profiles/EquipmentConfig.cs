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
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<EquipmentRow, EquipmentBO>()
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description)
                )
                .ForMember(
                    dest => dest.DrawingNumber,
                    opt => opt.MapFrom(src => src.DrawingNumber)
                )
                .ForMember(
                    dest => dest.EquipmentName,
                    opt => opt.MapFrom(src => src.Equipment)
                )
                .ForMember(
                    dest => dest.Priority,
                    opt => opt.MapFrom(src => src.Priority)
                )
                .ForMember(
                    dest => dest.Quantity,
                    opt => opt.MapFrom(src => src.Quantity)
                )
                .ForMember(
                    dest => dest.ReleaseDate,
                    opt => opt.MapFrom(src => src.ReleaseDate)
                )
                .ForMember(
                    dest => dest.ShippingTagNumber,
                    opt => opt.MapFrom(src => src.ShippingTagNumber)
                )
                .ForMember(
                    dest => dest.UnitWeight,
                    opt => opt.MapFrom(src => src.UnitWeight)
                )
                .ForMember(
                    dest => dest.WorkOrderNumber,
                    opt => opt.MapFrom(src => src.WorkOrderNumber)
                );

        }
    }
}