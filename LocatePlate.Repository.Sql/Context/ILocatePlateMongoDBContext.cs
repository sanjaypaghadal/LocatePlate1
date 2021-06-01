using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Repository
{
    public interface ILocatePlateMongoDBContext<T>
    {
        ICollection<T> ReadOnly(FilterDefinition<T> filter);
        ICollection<T> ReadOnly(FilterDefinition<T> filter, ProjectionDefinition<T> projectionDefinition);
        Task<ICollection<T>> ReadOnlyAsync(FilterDefinition<T> filter);
        long RecordCount(); // TODO: The previously existing ASYNC Count methods were wrongly named and should have been named CountAsync instead - but now refactoring that change alone wil mess up loads of files so will do that on a separate commit
        long RecordCount(FilterDefinition<T> filter);
        Task<long> Count();
        Task<long> Count(FilterDefinition<T> filter);
        void Write(T entity);
        void SafeWrite(T entity);
        void WriteAsync(T entity);
        T Update(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, FindOneAndUpdateOptions<T> options = null);
        Task<T> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, FindOneAndUpdateOptions<T> options = null);
        T Replace(FilterDefinition<T> filter, T entity);
        Task<T> ReplaceAsync(FilterDefinition<T> filter, T entity);
        void Remove(FilterDefinition<T> filter);
        DeleteResult DeleteAll();
        Task<DeleteResult> DeleteAllAsync();
        T Upsert(FilterDefinition<T> filter, T entity);
        List<BsonDocument> Aggregate(PipelineDefinition<T, BsonDocument> pipeline);
    }
}
