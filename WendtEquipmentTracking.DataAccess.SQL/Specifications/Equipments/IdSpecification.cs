using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class IdSpecification : Specification<Equipment> {

        private readonly int id;

        public IdSpecification(int id) {
            this.id = id;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.EquipmentId == id;
        }
    }
}
