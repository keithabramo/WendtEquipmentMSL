using Newtonsoft.Json.Converters;

namespace WendtEquipmentTracking.App.Common
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "M/d/yy";
        }
    }
}