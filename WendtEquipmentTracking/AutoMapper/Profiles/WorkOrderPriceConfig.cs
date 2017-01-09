using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class WorkOrderPriceConfig : Profile
    {
        public WorkOrderPriceConfig()
        {

            base.CreateMap<WorkOrderPriceBO, WorkOrderPriceModel>();
            base.CreateMap<WorkOrderPriceModel, WorkOrderPriceBO>();

            base.CreateMap<WorkOrderPriceBO, WorkOrderPriceSelectionModel>();
            base.CreateMap<WorkOrderPriceSelectionModel, WorkOrderPriceBO>();
        }
    }
}