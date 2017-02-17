using WendtEquipmentTracking.DataAccess.SQL.Specifications.Users;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class UserSpecs
    {

        public static Specification<User> Username(string username)
        {
            return new UsernameSpecification(username);
        }
    }
}
