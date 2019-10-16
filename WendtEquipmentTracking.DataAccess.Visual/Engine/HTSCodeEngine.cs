using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.DataAccess.Visual.Api;

namespace WendtEquipmentTracking.DataAccess.Visual
{
    public class HTSCodeEngine : IHTSCodeEngine
    {
        //private IRepository<HTSCode> repository = null;

        //public HTSCodeEngine(WendtEquipmentTrackingEntities dbContext)
        //{
            //this.repository = new Repository<Broker>(dbContext);
        //}

        public IQueryable<string> ListAll()
        {
            //return this.repository.FindAll();

            return new List<string>
            {
                "HTSCode1",
                "HTSCode2",
                "HTSCode3",
                "HTSCode4"
            }.AsQueryable();
        }
    }
}
