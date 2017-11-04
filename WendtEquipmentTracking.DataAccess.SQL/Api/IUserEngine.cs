using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IUserEngine
    {
        void AddNewUser(User user);
        void DeleteUser(User user);
        User Get(Specification<User> specification);
        IQueryable<User> List(Specification<User> specification);
        IQueryable<User> ListAll();
        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
        void UpdateUser(User user);
    }
}