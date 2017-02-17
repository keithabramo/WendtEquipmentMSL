using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Users
{

    internal class UsernameSpecification : Specification<User>
    {
        private string username;

        internal UsernameSpecification(string username)
        {
            this.username = username;
        }


        public override Expression<Func<User, bool>> IsSatisfiedBy()
        {
            return e => e.UserName == username;
        }
    }
}
