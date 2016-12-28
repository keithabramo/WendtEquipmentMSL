using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Api
{
    public interface IImportEngine
    {
        Import GetSheets(byte[] importFile);

        IEnumerable<ImportRow> GetEquipment(Import import);

    }
}