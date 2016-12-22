using AutoMapper;
using System;

namespace WendtEquipmentTracking.App.AutoMapper.Converters
{
    class DateTimeToNullableDateTimeConverter : ITypeConverter<DateTime, DateTime?> {
        
        public DateTime? Convert(DateTime source, DateTime? destination, ResolutionContext context)
        {
            DateTime? returnValue = new DateTime?();
            if (source != DateTime.MinValue)
            {
                returnValue = source;
            }

            return returnValue;
        }
    }
}
