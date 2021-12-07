using System;
using Dev.Data.Npgsql;
using Dev.Core.Entities;
using Dev.Npgsql.Repository;
using System.Threading.Tasks;

namespace Dev.Npgsql.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        INpgsqlRepository<T> GetRepository<T>() where T : BaseEntity, IEntity;

        bool BeginNewTransaction();
        Task<bool> BeginNewTransactionAsync();
        bool RollBackTransaction();
        Task<bool> RollBackTransactionAsync();

        int SaveChanges();
        Task<int> SaveChangesAsync();


        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
