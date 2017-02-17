using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IUserService
    {
        void Delete();
        UserBO GetCurrentUser();
        void Save(int projectId);
        void Update(int projectId);
    }
}