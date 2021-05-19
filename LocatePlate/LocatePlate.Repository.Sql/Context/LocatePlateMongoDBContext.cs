using LocatePlate.Infrastructure.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.Repository
{
    public class LocatePlateMongoDBContext<T> : ILocatePlateMongoDBContext<T>
        where T : BaseEntityMongoDB
    {
        private readonly IMongoClient _mongoClient;
        private readonly string _database;
        private readonly string _collection;

        public LocatePlateMongoDBContext(IMongoClient mongoClient, string database)
        {
            this._mongoClient = mongoClient;
            this._database = database;
            this._collection = typeof(T).Name;
        }

        public ICollection<T> ReadOnly(FilterDefinition<T> filter)
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindSync(filter).ToList();
        }

        public ICollection<T> ReadOnly(FilterDefinition<T> filter, ProjectionDefinition<T> projectionDefination)
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).Find(filter).Project<T>(projectionDefination).ToList();
        }

        public async Task<ICollection<T>> ReadOnlyAsync(FilterDefinition<T> filter)
        {
            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindSync(filter).ToListAsync();
        }

        public long RecordCount()
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).WithReadPreference(ReadPreference.Primary).CountDocuments(new BsonDocument());
        }

        public long RecordCount(FilterDefinition<T> filter)
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).CountDocuments(filter);
        }

        public async Task<long> Count()
        {
            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).CountAsync(new BsonDocument());
        }

        public async Task<long> Count(FilterDefinition<T> filter)
        {
            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).CountAsync(filter);
        }

        public void Write(T entity)
        {
            this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).InsertOne(entity);
        }

        public void SafeWrite(T entity)
        {
            this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).WithWriteConcern(WriteConcern.WMajority).InsertOne(entity);
        }

        public void WriteAsync(T entity)
        {
            this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).InsertOneAsync(entity);
        }

        public T Replace(FilterDefinition<T> filter, T entity)
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndReplace(filter, entity);
        }

        public T Update(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, FindOneAndUpdateOptions<T> options = null)
        {
            if (options == null)
                options = new FindOneAndUpdateOptions<T>() { ReturnDocument = ReturnDocument.After };

            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndUpdate(filter, updateDefinition, options);
        }

        public async Task<T> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, FindOneAndUpdateOptions<T> options = null)
        {
            if (options == null)
                options = new FindOneAndUpdateOptions<T>() { ReturnDocument = ReturnDocument.After };

            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndUpdateAsync(filter, updateDefinition, options);
        }

        public async Task<T> ReplaceAsync(FilterDefinition<T> filter, T entity)
        {
            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndReplaceAsync(filter, entity);
        }

        public void Remove(FilterDefinition<T> filter)
        {
            this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndDelete(filter);
        }

        public DeleteResult DeleteAll()
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).DeleteMany(new BsonDocument());
        }

        public async Task<DeleteResult> DeleteAllAsync()
        {
            return await this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).DeleteManyAsync(new BsonDocument());
        }

        public T Upsert(FilterDefinition<T> filter, T entity)
        {
            return this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).FindOneAndReplace(filter, entity, new FindOneAndReplaceOptions<T>() { IsUpsert = true });
        }

        public List<BsonDocument> Aggregate(PipelineDefinition<T, BsonDocument> pipeline)
        {
            var result = this._mongoClient.GetDatabase(this._database).GetCollection<T>(this._collection).Aggregate<BsonDocument>(pipeline);
            var bsonDocuments = result.ToListAsync().Result;
            return bsonDocuments;
        }
    }
}
