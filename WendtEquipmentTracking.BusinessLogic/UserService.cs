using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class UserService : IUserService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IUserEngine userEngine;
        private IProjectEngine projectEngine;

        public UserService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            userEngine = new UserEngine(dbContext);
            projectEngine = new ProjectEngine(dbContext);
        }

        public void Save(int projectId)
        {
            var user = new User
            {
                ProjectId = projectId,
                UserName = ActiveDirectoryHelper.CurrentUserUsername()
            };

            userEngine.AddNewUser(user);

            dbContext.SaveChanges();
        }

        public UserBO GetCurrentUser()
        {
            var username = ActiveDirectoryHelper.CurrentUserUsername();

            var user = userEngine.Get(UserSpecs.Username(username));

            UserBO userBO = null;

            if (user != null)
            {
                if (projectEngine.Get(ProjectSpecs.Id(user.ProjectId)) == null)
                {
                    this.Delete();
                }
                else
                {
                    userBO = new UserBO
                    {
                        ProjectId = user.ProjectId,
                        UserName = user.UserName
                    };
                }
            }

            return userBO;
        }

        public void Update(int projectId)
        {
            var username = ActiveDirectoryHelper.CurrentUserUsername();

            var oldUser = userEngine.Get(UserSpecs.Username(username));

            oldUser.ProjectId = projectId;

            userEngine.UpdateUser(oldUser);

            dbContext.SaveChanges();
        }

        public void Delete()
        {
            var username = ActiveDirectoryHelper.CurrentUserUsername();

            var user = userEngine.Get(UserSpecs.Username(username));

            if (user != null)
            {
                userEngine.DeleteUser(user);
            }

            dbContext.SaveChanges();
        }
    }
}
