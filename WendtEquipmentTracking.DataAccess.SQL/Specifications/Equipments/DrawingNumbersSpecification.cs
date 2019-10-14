using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class DrawingNumbersSpecification : Specification<Equipment>
    {

        private readonly IEnumerable<string> drawingNumbers;

        public DrawingNumbersSpecification(IEnumerable<string> drawingNumbers)
        {
            this.drawingNumbers = drawingNumbers;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => drawingNumbers.Contains(e.DrawingNumber);
        }
    }
}
