using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IPriorityEngine
    {
        IEnumerable<Priority> ListAll();

        Priority Get(Specification<Priority> specification);

        IEnumerable<Priority> List(Specification<Priority> specification);

        void AddNewPriority(Priority priority);

        void AddAllNewPriority(IEnumerable<Priority> prioritys);

        void UpdatePriority(Priority priority);

        void DeletePriority(Priority priority);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
