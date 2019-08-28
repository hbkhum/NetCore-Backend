using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Core
{
    public interface IGenericRepositoryEntities<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string whereValue, string orderBy);
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void Save();
    }
}