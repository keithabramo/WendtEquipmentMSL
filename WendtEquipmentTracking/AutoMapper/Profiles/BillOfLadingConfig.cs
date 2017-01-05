using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class BillOfLadingConfig : Profile
    {
        public BillOfLadingConfig()
        {

            base.CreateMap<BillOfLadingBO, BillOfLadingModel>();
            base.CreateMap<BillOfLadingModel, BillOfLadingBO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<BillOfLadingEquipmentBO, BillOfLadingEquipmentModel>().MaxDepth(1);
            base.CreateMap<BillOfLadingEquipmentModel, BillOfLadingEquipmentBO>().MaxDepth(1);
        }
    }
}