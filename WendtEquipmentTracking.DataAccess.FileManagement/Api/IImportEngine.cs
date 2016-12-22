using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        IEnumerable<string> GetSheets(byte[] importFile);

        IEnumerable<EquipmentRow> GetEquipment(byte[] importFile);

    }
}