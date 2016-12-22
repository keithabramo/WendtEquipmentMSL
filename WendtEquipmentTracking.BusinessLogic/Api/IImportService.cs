using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IImportService
    {
        IEnumerable<string> GetSheets(ImportBO userBO);
    }
}