using AutoMapper;
using System;
using WendtEquipmentTracking.App.AutoMapper.Converters;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class ProjectConfig : Profile {
        public ProjectConfig() {

            base.CreateMap<ProjectBO, ProjectModel>();
            base.CreateMap<ProjectModel, ProjectBO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
        }
    }
}