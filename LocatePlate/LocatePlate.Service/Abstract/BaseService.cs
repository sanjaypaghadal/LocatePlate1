using LocatePlate.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatePlate.Service.Abstract
{
    public abstract class BaseService<T, TEr> : IBaseService<T>
        where T : BaseEntity
        where TEr : IBaseRepository<T>
    {
        protected readonly TEr Repository;
        protected BaseService(TEr repository)
        {
            Repository = repository;
        }

        public virtual T Get(int id) => Repository.Get(id);
        public virtual IEnumerable<T> GetAll() => Repository.GetAll();
        public virtual PaginatedList<T> GetAll(int page, int pageSize) => Repository.GetAll(page, pageSize);
        public virtual PaginatedList<T> GetAllByUser(Guid userId, int page, int pageSize) => Repository.GetAllByUser(userId, page, pageSize);
        public virtual IEnumerable<T> GetAllByIds(List<int> ids) => Repository.GetAllByIds(ids);
        public virtual IEnumerable<T> GetAllByUserIds(List<Guid> userIds) => Repository.GetAllByUserIds(userIds);
        public virtual PaginatedList<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId, int page, int pageSize) => Repository.GetAllByUserAndRestaurant(userId, restaurantId, page, pageSize);
        public virtual IEnumerable<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId) => Repository.GetAllByUserAndRestaurant(userId, restaurantId);
        public virtual T Insert(T entity) => Repository.Insert(entity);
        public virtual T Update(T entity) => Repository.Update(entity);
        public virtual T Delete(T entity) => Repository.Delete(entity);
        public virtual T[] Delete(T[] entity) => Repository.Delete(entity);
        public virtual T Remove(T entity) => Repository.Remove(entity);
        public virtual T SoftDelete(T entity) => Repository.SoftDelete(entity);
        public virtual void SaveChanges() => Repository.SaveChanges();

        public virtual async Task<T> GetAsync(int id) => await Repository.GetAsync(id);
        public virtual async Task<IAsyncEnumerable<T>> GetAllAsync() => await Repository.GetAllAsync();
        public virtual async Task<T> SaveAsync(T entity) => await Repository.SaveAsync(entity);
        public virtual async Task<T> SoftDeleteAsync(T entity) => await Repository.SoftDeleteAsync(entity);
        public void Dispose() { }

        public IEnumerable<T> GetAllByUser(Guid userId) => Repository.GetAllByUser(userId);

    }
}
