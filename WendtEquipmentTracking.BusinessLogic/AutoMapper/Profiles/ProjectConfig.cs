using AutoMapper;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class ProjectConfig : Profile
    {
        public ProjectConfig()
        {

            base.CreateMap<Project, ProjectBO>()
                .ForMember(dest => dest.BillOfLadings, opt => opt.MapFrom(src => src.BillOfLadings.Where(o => !o.IsDeleted)))
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Equipments.Where(o => !o.IsDeleted)))
                .ForMember(dest => dest.HardwareKits, opt => opt.MapFrom(src => src.HardwareKits.Where(o => !o.IsDeleted)))
                .ForMember(dest => dest.WorkOrderPrices, opt => opt.MapFrom(src => src.WorkOrderPrices.Where(o => !o.IsDeleted)))
                .MaxDepth(2);

            base.CreateMap<ProjectBO, Project>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        }
    }
}