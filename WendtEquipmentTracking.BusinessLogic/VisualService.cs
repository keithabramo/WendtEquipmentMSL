
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.DataAccess.Visual;
using WendtEquipmentTracking.DataAccess.Visual.Api;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class VisualService : IVisualService
    {
        //private WendtEquipmentTrackingEntities dbContext;
        private IHTSCodeEngine htsCodeEngine;

        public VisualService()
        {
            //dbContext = new WendtEquipmentTrackingEntities();
            //htsCodeEngine = new HTSCodeEngine(dbContext);
            htsCodeEngine = new HTSCodeEngine();
        }

        public IEnumerable<string> GetAllHTSCodes()
        {
            var visuals = htsCodeEngine.ListAll();

            return visuals.ToList();
        }
    }
}
