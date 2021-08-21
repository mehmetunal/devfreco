using Dev.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Core.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        #region GET

        IQueryable<T> Get();
        Task<IEnumerable<T>> GetAsync();

        IEnumerable<T> FindAll(Expression<Func<T, bool>> where);
        IList<T> FindAll(Func<IQueryable<T>, IQueryable<T>> func = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> where);
        Task<IList<T>> FindAllAsync(Func<IQueryable<T>, IQueryable<T>> func = null);

        T Find(Expression<Func<T, bool>> where);
        Task<T> FindAsync(Expression<Func<T, bool>> where);
        T FindById(object id);
        Task<T> FindByIdAsync(object id);


        T SingleOrDefault(Expression<Func<T, bool>> @where);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> @where);
        T SingleById(object id);
        Task<T> SingleByIdAsync(object id);

        #endregion

        #region ADD

        T Add(T entity);
        Task<T> AddAsync(T entity);
        void AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        #endregion

        #region UPDATE

        T Update(T entity);
        Task<T> UpdateAsync(T entity);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);

        #endregion

        #region DELETE

        T Delete(T entity);
        Task<T> DeleteAsync(T entity);

        void Delete(Expression<Func<T, bool>> where);
        Task DeleteAsync(Expression<Func<T, bool>> where);

        void Delete(IEnumerable<T> entities);
        Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities);

        T Delete(object id);
        Task<T> DeleteAsync(object id);

        #endregion
    }
}
