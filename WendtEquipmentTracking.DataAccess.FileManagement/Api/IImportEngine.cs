using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine : IFileEngine
    {

        IEnumerable<EquipmentRow> GetEquipment(IDictionary<string, string> filePaths);

        IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(string filePath);

        IEnumerable<RawEquipmentRow> GetRawEquipment(string filePath);

        IEnumerable<PriorityRow> GetPriorities(string filePath);

        IEnumerable<VendorRow> GetVendors(string filePath);

        IEnumerable<BrokerRow> GetBrokers(string filePath);

        IEnumerable<HardwareCommercialCodeRow> GetHardwareCommercialCodes(string filePath);

    }
}