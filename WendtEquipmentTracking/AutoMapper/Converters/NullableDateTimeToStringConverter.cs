using AutoMapper;
using System;

namespace WendtEquipmentTracking.App.AutoMapper.Converters
{
    class NullableDateTimeToStringConverter : ITypeConverter<DateTime?, string> {

        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source.HasValue ? source.Value.ToShortDateString() : string.Empty;
        }
    }
}
