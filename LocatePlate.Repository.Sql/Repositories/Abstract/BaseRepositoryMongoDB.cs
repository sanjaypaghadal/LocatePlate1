using LocatePlate.Infrastructure.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.Repository.Repositories.Abstract
{
    public abstract class BaseRepositoryMongoDB<T> : IBaseRepositoryMongoDB<T> where T : BaseEntityMongoDB
    {
        protected readonly ILocatePlateMongoDBContext<T> _locatePlateMongoContext;

        protected BaseRepositoryMongoDB(ILocatePlateMongoDBContext<T> locatePlateMongoContext)
        {
            this._locatePlateMongoContext = locatePlateMongoContext;
        }

        public virtual long Count()
        {
            return this._locatePlateMongoContext.RecordCount();
        }

        public virtual async Task<long> CountAsync()
        {
            return await this._locatePlateMongoContext.Count();
        }

        public virtual void Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            this._locatePlateMongoContext.Remove(filter);
        }

        public virtual void DeleteById(int id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            this._locatePlateMongoContext.Remove(filter);
        }

        public void DeleteAll()
        {
            this._locatePlateMongoContext.DeleteAll();
        }

        public virtual T Get(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var t = this._locatePlateMongoContext.ReadOnly(filter);
            return t?.SingleOrDefault();
        }

        public virtual T Get(int id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var t = this._locatePlateMongoContext.ReadOnly(filter);
            return t?.SingleOrDefault();
        }

        public virtual ICollection<T> GetAll()
        {
            var filter = Builders<T>.Filter.Empty;
            return this._locatePlateMongoContext.ReadOnly(filter).ToList();
        }


        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Empty;
            return await this._locatePlateMongoContext.ReadOnlyAsync(filter);
        }

        public virtual async Task<PaginatedList<T>> GetAllAsync(int page, int pageSize)
        {
            var filter = Builders<T>.Filter.Empty;
            var result = await this._locatePlateMongoContext.ReadOnlyAsync(filter);

            return new PaginatedList<T>(result.AsQueryable(), page, pageSize);
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var t = await this._locatePlateMongoContext.ReadOnlyAsync(filter);
            return t?.SingleOrDefault();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var t = await this._locatePlateMongoContext.ReadOnlyAsync(filter);
            return t?.SingleOrDefault();
        }

        public T Save(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");

            this._locatePlateMongoContext.Write(entity);
            return entity;
        }

        public T SaveWithAcknowledgement(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");
            this._locatePlateMongoContext.SafeWrite(entity);
            return entity;
        }

        public virtual async Task<T> SaveAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");
            this._locatePlateMongoContext.WriteAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");

            var filter = Builders<T>.Filter.And(
                 Builders<T>.Filter.Eq("_id", entity.Id));

            // Concurrency check
            var previousEntity = this._locatePlateMongoContext.Replace(filter, entity);
            if (previousEntity == null)
            {
                throw new Exception();
            }
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");

            var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq("_id", entity.Id));

            // Concurrency check
            var previousEntity = await this._locatePlateMongoContext.ReplaceAsync(filter, entity);
            if (previousEntity == null)
            {
                throw new Exception();
            }
            return entity;
        }

        public T Upsert(T entity)
        {
            if (entity == null)
                throw new ArgumentException("entity is null");

            var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq("_id", entity.Id));

            // Concurrency check
            this._locatePlateMongoContext.Upsert(filter, entity);
            return entity;
        }

        public void Aggregate(T entity)
        {
            var pipeline = PipelineDefinition<T, BsonDocument>.Create(new BsonDocument());
            this._locatePlateMongoContext.Aggregate(pipeline);
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
        // ~BaseRepository() {
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
