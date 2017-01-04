using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class BillOfLandingConfig : Profile
    {
        public BillOfLandingConfig()
        {

            base.CreateMap<BillOfLanding, BillOfLandingBO>();
            base.CreateMap<BillOfLandingBO, BillOfLanding>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<BillOfLandingEquipmentBO, BillOfLandingEquipment>();
            base.CreateMap<BillOfLandingEquipment, BillOfLandingEquipmentBO>().MaxDepth(1);
        }
    }
}