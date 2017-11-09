using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private WendtEquipmentTrackingEntities entities;
        private DbSet<T> table = null;

        public Repository()
        {
            this.entities = new WendtEquipmentTrackingEntities();

            //Done to prevent blocking -- this COULD make dirty reads but is unlikely in this application to matter
            entities.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
            table = entities.Set<T>();
        }

        public Repository(WendtEquipmentTrackingEntities entities)
        {
            this.entities = entities;
            entities.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");

            table = entities.Set<T>();
        }

        public WendtEquipmentTrackingEntities GetContext()
        {
            return entities;
        }

        /// <summary>
        /// Gets all records as an IEnumberable
        /// </summary>
        /// <returns>An IEnumberable object containing the results of the query</returns>
        public IQueryable<T> GetAll()
        {
            return table;
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A collection containing the results of the query</returns>
        public IQueryable<T> Find(Specification<T> specification)
        {
            return table.Where<T>(specification.IsSatisfiedBy());
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public T Single(Specification<T> specification)
        {
            return table.SingleOrDefault<T>(specification.IsSatisfiedBy());
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public T First(Specification<T> specification)
        {
            return table.FirstOrDefault<T>(specification.IsSatisfiedBy());
        }

        /// <summary>
        /// Deletes the specified entitiy
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            table.Remove(entity);
        }

        /// <summary>
        /// Inserts the specified entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            table.Add(entity);
        }

        /// <summary>
        /// Updates the specified entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        public void Update(T entity)
        {
            table.Attach(entity);
            entities.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (entities != null)
                {
                    entities.Dispose();
                    entities = null;
                }
            }
        }

        /// <summary>
        /// Exectutes a stored procedure
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        public int ExectuteStoredProcedure(string procedureName, List<SqlParameter> parameters)
        {
            string parameterNameList = buildParamNameList(parameters);

            var result = entities.Database.ExecuteSqlCommand("exec " + procedureName + " " + parameterNameList, parameters.ToArray());

            return result;
        }

        private string buildParamNameList(List<SqlParameter> parameters)
        {
            string parameterNameList = "";
            foreach (var parameter in parameters)
            {
                if (!String.IsNullOrWhiteSpace(parameterNameList))
                {
                    parameterNameList += ",";
                }
                parameterNameList += "@" + parameter.ParameterName + " = @" + parameter.ParameterName;
            }

            return parameterNameList;
        }
    }


}
