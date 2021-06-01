using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Infrastructure.Domain
{
    public interface IBaseRepository<T> : IDisposable where T : BaseEntity
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        PaginatedList<T> GetAll(int page, int pageSize);
        IEnumerable<T> GetAllByUser(Guid userId);
        PaginatedList<T> GetAllByUser(Guid userId, int page, int pageSize);
        IEnumerable<T> GetAllByIds(List<int> ids);
        IEnumerable<T> GetAllByUserIds(List<Guid> userIds);
        PaginatedList<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId, int page, int pageSize);
        IEnumerable<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId);
        T Insert(T entity);
        T Update(T entity);
        T Delete(T entity);
        T[] Delete(T[] entity);
        T Remove(T entity);
        T SoftDelete(T entity);
        void SaveChanges();
        Task<T> GetAsync(int id);
        Task<IAsyncEnumerable<T>> GetAllAsync();
        Task<T> SaveAsync(T entity);
        Task<T> SoftDeleteAsync(T entity);
    }
}
