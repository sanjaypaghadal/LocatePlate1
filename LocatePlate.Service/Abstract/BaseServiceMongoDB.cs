using LocatePlate.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Service.Abstract
{
    public abstract class BaseServiceMongoDB<T, TEr> : IBaseServiceMongoDB<T>
       where T : BaseEntityMongoDB
       where TEr : IBaseRepositoryMongoDB<T>
    {
        protected readonly TEr Repository;
        protected BaseServiceMongoDB(TEr repository)
        {
            Repository = repository;
        }

        public virtual async Task<long> CountAsync()
        {
            return await Repository.CountAsync();
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void DeleteAll() => Repository.DeleteAll();

        public T Get(Guid id)
        {
            return Repository.Get(id);
        }

        public T Get(int id)
        {
            return Repository.Get(id);
        }

        public ICollection<T> GetAll()
        {
            return Repository.GetAll();
        }

        public Task<ICollection<T>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }
        public virtual async Task<PaginatedList<T>> GetAllAsync(int page, int pageSize) => await Repository.GetAllAsync(page, pageSize);

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await Repository.GetAsync(id);
        }

        public T Save(T entity)
        {
            return Repository.Save(entity);
        }

        public virtual async Task<T> SaveAsync(T entity)
        {
            return await Repository.SaveAsync(entity);
        }

        public T Update(T entity)
        {
            return Repository.Update(entity);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await Repository.UpdateAsync(entity);
        }

        public T Upsert(T entity)
        {
            return Repository.Upsert(entity);
        }

        public void Aggregate(T entity)
        {
            Repository.Aggregate(entity);
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
