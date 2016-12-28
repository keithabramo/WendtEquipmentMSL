using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class ImportBO
    {
        public string FileName { get; set; }

        public IEnumerable<string> Sheets { get; set; }

    }
}