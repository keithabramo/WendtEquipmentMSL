using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class Import
    {
        public IEnumerable<string> Sheets { get; set; }

        public string FileName { get; set; }
    }
}