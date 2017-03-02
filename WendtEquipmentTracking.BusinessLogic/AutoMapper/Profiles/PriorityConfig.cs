using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class PriorityConfig : Profile
    {
        public PriorityConfig()
        {

            base.CreateMap<Priority, PriorityBO>();
            base.CreateMap<PriorityBO, Priority>();

        }
    }
}