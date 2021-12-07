﻿using System;
using Dev.Data.Npgsql;
using Dev.Core.Entities;
using Dev.Npgsql.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dev.Npgsql.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Variables

        private readonly DbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        #endregion

        #region Ctor

        public UnitOfWork(DbContext context)
        {
            this._context = context;
        }

        #endregion

        #region BusinessSection

        public INpgsqlRepository<T> GetRepository<T>() where T : BaseEntity, IEntity
            => new NpgsqlRepository<T>(_context);

        public bool BeginNewTransaction()
        {
            try
            {
                _transaction = _context.Database.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> BeginNewTransactionAsync()
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool RollBackTransaction()
        {
            try
            {
                _transaction.Rollback();
                _transaction = null;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> RollBackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
                _transaction = null;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int SaveChanges()
        {
            var transaction = _transaction ?? _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    if (_context == null)
                    {
                        throw new ArgumentException($"Context is null");
                    }

                    var result = _context.SaveChanges();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Error on save changes " + ex.InnerException.Message, ex);
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var transaction = _transaction ?? await _context.Database.BeginTransactionAsync();
            await using (transaction)
            {
                try
                {
                    if (_context == null)
                    {
                        throw new ArgumentException($"Context is null");
                    }

                    var result = await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error on save changes ", ex);
                }
            }
        }


        public void Commit()
        {
            _transaction.Commit();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        #endregion

        #region DisposingSection

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
