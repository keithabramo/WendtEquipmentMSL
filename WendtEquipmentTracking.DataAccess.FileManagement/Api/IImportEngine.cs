using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        string SaveFile(byte[] equipmentFile);

        IEnumerable<EquipmentRow> GetEquipment(EquipmentImport import);

        IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(string filePath);

        IEnumerable<RawEquipmentRow> GetRawEquipment(string filePath);

        IEnumerable<PriorityRow> GetPriorities(string filePath);

    }
}