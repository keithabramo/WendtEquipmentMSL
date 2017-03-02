using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class PriorityConfig : Profile
    {
        public PriorityConfig()
        {

            base.CreateMap<PriorityBO, PriorityModel>();
            base.CreateMap<PriorityModel, PriorityBO>();
        }
    }
}