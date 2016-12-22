using System;

namespace WendtEquipmentTracking.Common
{
    public static class StringExtention {
        public static bool Contains(this string source, string toCheck, StringComparison comp) {
            return source != null && source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
