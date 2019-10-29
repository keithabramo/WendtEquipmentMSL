using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IVisualService
    {
        IEnumerable<HTSCodeBO> GetAllHTSCodes();
    }
}