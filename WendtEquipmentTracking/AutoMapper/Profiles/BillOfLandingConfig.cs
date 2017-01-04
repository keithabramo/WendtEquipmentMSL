using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class BillOfLandingConfig : Profile
    {
        public BillOfLandingConfig()
        {

            base.CreateMap<BillOfLandingBO, BillOfLandingModel>();
            base.CreateMap<BillOfLandingModel, BillOfLandingBO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<BillOfLandingEquipmentBO, BillOfLandingEquipmentModel>().MaxDepth(1);
            base.CreateMap<BillOfLandingEquipmentModel, BillOfLandingEquipmentBO>();
        }
    }
}