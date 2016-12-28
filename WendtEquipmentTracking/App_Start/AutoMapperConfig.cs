using AutoMapper;
using WendtEquipmentTracking.App.AutoMapper;
using WendtEquipmentTracking.BusinessLogic.AutoMapper;

namespace WendtEquipmentTracking.App
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
                cfg.AddProfiles(new[] {
                    "WendtEquipmentTracking.App",
                    "WendtEquipmentTracking.BusinessLogic"
            }));
        }
    }
}