using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IUserEngine
    {
        void AddNewUser(User user);
        void DeleteUser(User user);
        User Get(Specification<User> specification);
        IEnumerable<User> List(Specification<User> specification);
        IEnumerable<User> ListAll();
        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
        void UpdateUser(User user);
    }
}