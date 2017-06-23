using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IBillOfLadingEngine
    {
        IEnumerable<BillOfLading> ListAll();

        BillOfLading Get(Specification<BillOfLading> specification);

        IEnumerable<BillOfLading> List(Specification<BillOfLading> specification);

        void AddNewBillOfLading(BillOfLading billOfLading);

        void UpdateBillOfLading(BillOfLading billOfLading);

        void DeleteBillOfLading(BillOfLading billOfLading);

        void UpdateRTS(int billOfLadingId);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
