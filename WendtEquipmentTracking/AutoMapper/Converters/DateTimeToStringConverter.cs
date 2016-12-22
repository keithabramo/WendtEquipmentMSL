using AutoMapper;
using System;

namespace WendtEquipmentTracking.App.AutoMapper.Converters
{
    class DateTimeToStringConverter : ITypeConverter<DateTime, string> {

        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToShortDateString();
        }
    }
}
