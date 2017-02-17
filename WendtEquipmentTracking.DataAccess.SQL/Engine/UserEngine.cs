using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Api;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class UserEngine : IUserEngine
    {
        private IRepository<User> repository = null;

        public UserEngine()
        {
            this.repository = new Repository<User>();
        }

        public UserEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<User>(dbContext);
        }

        public UserEngine(Repository<User> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<User> ListAll()
        {
            return this.repository.GetAll();
        }

        public User Get(Specification<User> specification)
        {
            return this.repository.Single(specification);
        }

        public IEnumerable<User> List(Specification<User> specification)
        {
            return this.repository.Find(specification);
        }

        public void AddNewUser(User user)
        {
            this.repository.Insert(user);
            this.repository.Save();
        }

        public void UpdateUser(User user)
        {

            this.repository.Update(user);
            this.repository.Save();
        }

        public void DeleteUser(User user)
        {

            this.repository.Delete(user);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<User>(dbContext);
        }
    }
}
