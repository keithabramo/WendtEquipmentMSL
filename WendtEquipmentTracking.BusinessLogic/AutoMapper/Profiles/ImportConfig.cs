using AutoMapper;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.BusinessLogic.AutoMapper.Profiles
{
    public class ImportConfig : Profile
    {
        public ImportConfig()
        {
            base.CreateMap<EquipmentImport, EquipmentImportBO>();
            base.CreateMap<EquipmentImportBO, EquipmentImport>();

        }
    }
}