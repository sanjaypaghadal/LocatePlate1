using LocatePlate.Infrastructure.Domain;
using LocatePlate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocatePlate.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly LocatePlateContext _context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        protected BaseRepository(LocatePlateContext context)
        {
            this._context = context;
            entities = context.Set<T>();
        }
        public virtual T Get(int id) => entities.SingleOrDefault(s => s.Id == id);
        public virtual IEnumerable<T> GetAll() => entities.AsEnumerable();
        public virtual PaginatedList<T> GetAll(int page, int pageSize) => new PaginatedList<T>(entities.AsQueryable(), page, pageSize);
        public virtual IEnumerable<T> GetAllByUser(Guid userId) => entities.Where(s => s.UserId == userId).AsEnumerable();
        public virtual IEnumerable<T> GetAllByIds(List<int> ids) => entities.Where(s => ids.Contains(s.Id)).AsEnumerable();
        public virtual IEnumerable<T> GetAllByUserIds(List<Guid> userIds) => entities.Where(s => userIds.Contains(s.UserId)).AsEnumerable();
        public virtual PaginatedList<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId, int page, int pageSize) => new PaginatedList<T>(entities.Where(c => c.UserId == userId && c.RestaurantId == restaurantId).AsQueryable(), page, pageSize);
        public virtual IEnumerable<T> GetAllByUserAndRestaurant(Guid userId, int restaurantId) => entities.Where(c => c.UserId == userId && c.RestaurantId == restaurantId).AsEnumerable();

        public virtual PaginatedList<T> GetAllByUser(Guid userId, int page, int pageSize)
        {
            var result = entities.Where(s => s.UserId == userId && s.IsSoftDelete != true);
            var paginatedResult = new PaginatedList<T>(result, page, pageSize);

            return paginatedResult;
        }
        public virtual T Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public virtual T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public virtual T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual T[] Delete(T[] entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.RemoveRange(entity);
            _context.SaveChanges();
            return entity;
        }
        public virtual T Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Remove(entity);
            return entity;
        }
        public virtual T SoftDelete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entity.IsSoftDelete = true;
            _context.SaveChanges();
            return entity;
        }
        public virtual void SaveChanges() => _context.SaveChanges();

        public virtual async Task<T> GetAsync(int id) => await entities.SingleOrDefaultAsync(s => s.Id == id);
        public virtual async Task<IAsyncEnumerable<T>> GetAllAsync() => await (Task<IAsyncEnumerable<T>>)entities.AsAsyncEnumerable();
        public virtual async Task<T> SaveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.Id == 0)
                entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<T> SoftDeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.IsSoftDelete = true;
            await _context.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<TSource> DistinctBy<TSource, TKey>
      (IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public void Dispose() { }
    }
}
