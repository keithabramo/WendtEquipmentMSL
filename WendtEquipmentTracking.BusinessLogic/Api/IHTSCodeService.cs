using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IVisualService
    {
        IEnumerable<string> GetAllHTSCodes();
    }
}