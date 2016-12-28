using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL
{
    internal interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        WendtEquipmentTrackingEntities GetContext();
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Specification<TEntity> specification);
        TEntity Single(Specification<TEntity> specification);
        TEntity First(Specification<TEntity> specification);
        void Insert(TEntity entity);
        void InsertAll(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Save();
        IEnumerable<K> ExectuteStoredProcedure<K>(string procedureName, List<SqlParameter> parameters);
    }
}
