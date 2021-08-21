﻿using System.Linq;
using Dev.Data.Npgsql;
using Dev.Core.Entities;
using Dev.Core.Repository;
using System.Threading.Tasks;

namespace Dev.Npgsql.Repository
{
    public interface INpgsqlRepository<T> : IRepository<T> where T : BaseEntity, IEntity
    {
        #region CustomMethod
        IQueryable<T> AsNoTracking();
        IQueryable<T> FromSqlRaw(string sql, params object[] par);
        int Execute(string sql, params object[] par);
        Task<int> ExecuteAsync(string sql, params object[] par);
        #endregion
    }
}
