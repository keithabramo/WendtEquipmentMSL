using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class DescriptionSpecification : Specification<Equipment>
    {

        private readonly string description;

        public DescriptionSpecification(string description)
        {
            this.description = description;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.Description == description;
        }
    }
}
