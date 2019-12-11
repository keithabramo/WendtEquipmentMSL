using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IImportService
    {
        string SaveFile(byte[] file);

        IEnumerable<EquipmentBO> GetEquipmentImport(EquipmentImportBO importBO);

        IEnumerable<EquipmentBO> GetEquipmentRevisionImport(EquipmentRevisionImportBO importBO);

        IEnumerable<WorkOrderPriceBO> GetWorkOrderPricesImport(string filePath);

        IEnumerable<PriorityBO> GetPrioritiesImport(string filePath);

        IEnumerable<EquipmentBO> GetRawEquipmentImport(string filePath);

        IEnumerable<VendorBO> GetVendorsImport(string filePath);

        IEnumerable<BrokerBO> GetBrokersImport(string filePath);

        IEnumerable<HardwareCommercialCodeBO> GetHardwareCommercialCodesImport(string filePath);
    }
}