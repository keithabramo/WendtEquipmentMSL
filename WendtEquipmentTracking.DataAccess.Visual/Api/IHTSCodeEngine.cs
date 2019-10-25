using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.Visual.Api
{
    public interface IHTSCodeEngine
    {
        IEnumerable<string> ListAll();

    }
}
