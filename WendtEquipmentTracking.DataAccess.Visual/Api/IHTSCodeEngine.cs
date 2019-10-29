using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.Visual.Domain;

namespace WendtEquipmentTracking.DataAccess.Visual.Api
{
    public interface IHTSCodeEngine
    {
        IEnumerable<HTSCodeDTO> ListAll();

    }
}
