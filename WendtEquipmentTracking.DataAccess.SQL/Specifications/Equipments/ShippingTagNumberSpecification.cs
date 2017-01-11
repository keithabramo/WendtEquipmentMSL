using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class ShippingTagNumberSpecification : Specification<Equipment>
    {

        private readonly string shippingTagNumber;

        public ShippingTagNumberSpecification(string shippingTagNumber)
        {
            this.shippingTagNumber = shippingTagNumber;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.ShippingTagNumber == shippingTagNumber;
        }
    }
}
