using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IImportService
    {
        string SaveEquipmentFile(byte[] file);

        IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO);

        IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(byte[] file);
    }
}