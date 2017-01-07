using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IImportService
    {
        ImportBO GetSheets(byte[] file);

        IEnumerable<EquipmentBO> GetEquipmentImport(ImportBO importBO);

        IEnumerable<WorkOrderPriceBO> GetWorkOrderPriceImport(byte[] file);
    }
}