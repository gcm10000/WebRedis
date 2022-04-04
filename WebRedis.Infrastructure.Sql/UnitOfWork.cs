using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Infrastructure.Sql.DbContexts;

namespace WebRedis.Infrastructure.Sql
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;
        private readonly SqlDbContext _context;
        private IDbContextTransaction _dbContextTransaction;

        public IRepository<Product> ProductRepository { get; private set; }

        public UnitOfWork(SqlDbContext context, IRepository<Product> productRepository)
        {
            this._context = context;
            this.ProductRepository = productRepository;
        }

        public async Task BeginTransactionAsync()
        {
            _dbContextTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<bool> Commit()
        {
            bool returnValue = true;

            if (_dbContextTransaction == null)
                throw new Exception("Transação não iniciada.");

            try
            {
                await _context.SaveChangesAsync();
                await _dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                returnValue = false;
                _dbContextTransaction.Rollback();

            }
            return returnValue;
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
