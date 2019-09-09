using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IBrokerService
    {
        IEnumerable<int> SaveAll(IEnumerable<BrokerBO> brokerBOs);

        void Update(BrokerBO brokerBO);

        void UpdateAll(IEnumerable<BrokerBO> brokerBOs);

        void Delete(int id);

        IEnumerable<BrokerBO> GetAll();

        IEnumerable<BrokerBO> GetByIds(IEnumerable<int> ids);

        BrokerBO GetByName(string name);

    }
}