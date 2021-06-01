using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Infrastructure.Domain
{
    public interface IBaseRepositoryMongoDB<T> : IDisposable where T : BaseEntityMongoDB
    {
        T Get(Guid id);
        T Get(int id);
        ICollection<T> GetAll();
        T Save(T entity);
        T Update(T entity);
        void Delete(T entity);
        void DeleteAll();
        T Upsert(T entity);
        void Aggregate(T entity);

        // asynchronus methods
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<PaginatedList<T>> GetAllAsync(int page, int pageSize);
        Task<T> SaveAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<long> CountAsync();
    }
}
