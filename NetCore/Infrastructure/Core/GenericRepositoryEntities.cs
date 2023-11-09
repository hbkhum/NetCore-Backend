using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetCore.Infrastructure.Context;

namespace NetCore.Infrastructure.Core
{
    public abstract class GenericRepositoryEntities<T> : IGenericRepositoryEntities<T> where T : class
    {
        private readonly VehicleContext _entities;
        protected DbSet<T> Dbset;

        /// <summary>
        /// This a Contructor Generic Repository
        /// </summary>
        /// <param name="entities">Is a element from database</param>
        protected GenericRepositoryEntities(VehicleContext entities)
        {
            _entities = entities;
            Dbset = entities.Set<T>();
            //
        }

        public async Task<IEnumerable<T>> GetAllAsync(string whereValue, string orderBy)
        {
            if (whereValue == "") whereValue = "true";
            if (orderBy == "") orderBy = "true";
            return await Dbset.Where(whereValue).OrderBy(orderBy).ToListAsync();

            //var task = Task.Run(() =>
            //{
            //    var result = Dbset
            //        .Where(whereValue)
            //        .OrderBy(orderBy);
            //    //.Skip((pageIndex - 1) * pageSize)
            //    //.Take(pageSize);
            //    return result;
            //});
            //return (IEnumerable<T>)await task;
        }

        public async Task<IEnumerable<object>> StoredProcedure(string storedProcName, Dictionary<string, object> parameter)
        {
            var cmd = _entities.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            foreach (var p in parameter)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = p.Key;
                param.Value = p.Value;
                cmd.Parameters.Add(param);
            }
            var obj = new List<object>();
            using (cmd)
            {
                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();
                try
                {

                    using (var dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            var objField = new List<object>();
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                objField.Add(dr[i]);
                            }
                            obj.Add(objField);
                        }
                        //var k = dr[0];
                    }
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
            var task = Task.Run(() => obj);
            return await task;
        }

        public async Task StoredProcedure(string strSQL)
        {
            await _entities.Database.ExecuteSqlRawAsync(strSQL);
        }




        /// <summary>
        /// Custom search using expression Lambda
        /// </summary>
        /// <param name="predicate">Expretion type Lambda</param>
        /// <returns>Return a List type T</returns>
        public async Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await Dbset.Where(predicate).ToListAsync();
            //var task = Task.Run(() => Dbset.Where(predicate).ToList());
            //return await task;

        }

        /// <summary>
        /// Add to element to database
        /// </summary>
        /// <param name="entity">Is a class type T</param>
        /// <returns>Return element to add</returns>
        public void Add(T entity)
        {
            try
            {
                Dbset.Add(entity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public void Add(IEnumerable<T> entities)
        {
            try
            {
                Dbset.AddRange(entities);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// Edit element of database
        /// </summary>
        /// <param name="entity">Is a class type T</param>
        public void Edit(T entity)
        {
            try
            {
                //var properties = entity.GetType().GetProperties()
                //    .Where(p => p.GetGetMethod().IsVirtual).ToArray();
                //foreach (var p in properties)
                //{
                //    p.SetValue(entity, null);
                //}
                _entities.Entry(entity).State = EntityState.Modified;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete element of database
        /// </summary>
        /// <param name="entity">Is a class type T</param>
        /// <returns>Return a object type T</returns>
        public void Delete(T entity)
        {
            try
            {
                Dbset.Remove(entity);
                Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// Save element of database
        /// </summary>
        public void Save()
        {
            try
            {

                //using (var context = _entities)
                //{

                //    context.SaveChanges();
                //}

                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
