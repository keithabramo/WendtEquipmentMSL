using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IImportService
    {
        string SaveFile(byte[] file);

        IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO);

        IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(string filePath);
        IEnumerable<EquipmentBO> GetRawEquipmentImport(string filePath);
    }
}