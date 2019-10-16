using System.Linq;

namespace WendtEquipmentTracking.DataAccess.Visual.Api
{
    public interface IHTSCodeEngine
    {
        IQueryable<string> ListAll();

    }
}
