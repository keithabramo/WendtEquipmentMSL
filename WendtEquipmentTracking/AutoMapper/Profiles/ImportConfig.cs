using AutoMapper;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.AutoMapper.Profiles
{
    public class ImportConfig : Profile
    {
        public ImportConfig()
        {
            base.CreateMap<EquipmentImportModel, EquipmentImportBO>();
        }
    }
}