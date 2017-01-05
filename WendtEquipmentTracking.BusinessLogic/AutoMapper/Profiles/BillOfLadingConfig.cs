using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class BillOfLadingConfig : Profile
    {
        public BillOfLadingConfig()
        {

            base.CreateMap<BillOfLading, BillOfLadingBO>();
            base.CreateMap<BillOfLadingBO, BillOfLading>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            base.CreateMap<BillOfLadingEquipmentBO, BillOfLadingEquipment>();
            base.CreateMap<BillOfLadingEquipment, BillOfLadingEquipmentBO>().MaxDepth(1);
        }
    }
}