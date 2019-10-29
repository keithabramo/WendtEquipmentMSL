
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
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

        public IEnumerable<HTSCodeBO> GetAllHTSCodes()
        {
            var htsCodes = htsCodeEngine.ListAll();

            var htsCodeBOs = htsCodes.Select(x => new HTSCodeBO
            {
                HTSCode = x.HTSCode,
                Description = x.Description
            });

            return htsCodeBOs.ToList();
        }
    }
}
