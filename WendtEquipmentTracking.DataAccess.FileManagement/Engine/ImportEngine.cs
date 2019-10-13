using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;
using WendtEquipmentTracking.DataAccess.FileManagement.Helper;

namespace WendtEquipmentTracking.DataAccess.FileManagement
{
    public class ImportEngine : FileEngine, IImportEngine
    {

        public IEnumerable<EquipmentRow> GetEquipment(EquipmentImport import)
        {
            var equipmentData = ImportHelper.GetEquipment(import);

            return equipmentData;
        }

        public IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(string filePath)
        {
            var workOrderPriceRecords = ImportHelper.GetWorkOrderPrices(filePath);

            return workOrderPriceRecords;
        }

        public IEnumerable<RawEquipmentRow> GetRawEquipment(string filePath)
        {
            return ImportHelper.GetRawEquipment(filePath);
        }

        public IEnumerable<PriorityRow> GetPriorities(string filePath)
        {
            return ImportHelper.GetPriorities(filePath);
        }

        public IEnumerable<VendorRow> GetVendors(string filePath)
        {
            return ImportHelper.GetVendors(filePath);
        }

        public IEnumerable<BrokerRow> GetBrokers(string filePath)
        {
            return ImportHelper.GetBrokers(filePath);
        }

        public IEnumerable<HardwareCommercialCodeRow> GetHardwareCommercialCodes(string filePath)
        {
            return ImportHelper.GetHardwareCommercialCodes(filePath);
        }
    }
}
