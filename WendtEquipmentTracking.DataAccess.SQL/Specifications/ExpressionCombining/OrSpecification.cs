using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.ExpressionCombining
{

    /// <summary>
    /// The or specification.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    [Serializable]
    internal class OrSpecification<T> : Specification<T> {
        private readonly Specification<T> spec1;
        private readonly Specification<T> spec2;

        public OrSpecification(Specification<T> spec1, Specification<T> spec2) {
            this.spec1 = spec1;
            this.spec2 = spec2;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy() {
            var expr1 = spec1.IsSatisfiedBy();
            var expr2 = spec2.IsSatisfiedBy();

            // combines the expressions without the need for Expression.Invoke which fails on EntityFramework
            return expr1.OrElse(expr2);
        }
    }
}
