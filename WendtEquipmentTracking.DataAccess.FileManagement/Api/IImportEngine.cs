using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        string SaveEquipmentFile(byte[] equipmentFile);

        IEnumerable<EquipmentRow> GetEquipment(Import import);

        IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(byte[] importFile);

    }
}