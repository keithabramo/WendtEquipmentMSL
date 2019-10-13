using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IPriorityEngine
    {
        IQueryable<Priority> ListAll();

        Priority Get(Specification<Priority> specification);

        IQueryable<Priority> List(Specification<Priority> specification);

        void AddNewPriority(Priority priority);

        void AddNewPrioritys(IEnumerable<Priority> prioritys);

        void UpdatePriority(Priority priority);

        void UpdatePriorities(IEnumerable<Priority> priorities);

        void DeletePriority(Priority priority);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
