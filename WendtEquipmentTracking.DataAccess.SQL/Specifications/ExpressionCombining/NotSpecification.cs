
using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.ExpressionCombining
{

    internal class NotSpecification<T> : Specification<T> {
        private readonly Specification<T> spec;

        public NotSpecification(Specification<T> spec) {
            this.spec = spec;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy() {
            var isSatisfiedBy = spec.IsSatisfiedBy();
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(isSatisfiedBy.Body), isSatisfiedBy.Parameters);
        }
    }
}
