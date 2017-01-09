using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class WorkOrderPriceConfig : Profile
    {
        public WorkOrderPriceConfig()
        {

            base.CreateMap<WorkOrderPrice, WorkOrderPriceBO>();
            base.CreateMap<WorkOrderPriceBO, WorkOrderPrice>();

            base.CreateMap<WorkOrderPriceRow, WorkOrderPriceBO>();

        }
    }
}