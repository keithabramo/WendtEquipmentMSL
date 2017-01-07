using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        Import GetSheets(byte[] importFile);

        IEnumerable<EquipmentRow> GetEquipment(Import import);

        IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(byte[] importFile);

    }
}