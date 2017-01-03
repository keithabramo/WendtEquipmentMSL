using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IBillOfLandingEngine
    {
        IEnumerable<BillOfLanding> ListAll();

        BillOfLanding Get(Specification<BillOfLanding> specification);

        IEnumerable<BillOfLanding> List(Specification<BillOfLanding> specification);

        void AddNewBillOfLanding(BillOfLanding billOfLanding);

        void UpdateBillOfLanding(BillOfLanding billOfLanding);

        void DeleteBillOfLanding(BillOfLanding billOfLanding);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
