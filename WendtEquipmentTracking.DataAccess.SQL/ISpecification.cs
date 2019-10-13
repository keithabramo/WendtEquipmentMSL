using System;
namespace WendtEquipmentTracking.DataAccess.SQL
{
    internal interface ISpecification<T>
    {
        System.Linq.Expressions.Expression<Func<T, bool>> IsSatisfiedBy();
    }
}
