using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IBillOfLadingEngine
    {
        IQueryable<BillOfLading> ListAll();

        BillOfLading Get(Specification<BillOfLading> specification);

        IQueryable<BillOfLading> List(Specification<BillOfLading> specification);

        void AddNewBillOfLading(BillOfLading billOfLading);

        void UpdateBillOfLading(BillOfLading billOfLading);

        void DeleteBillOfLading(BillOfLading billOfLading);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
