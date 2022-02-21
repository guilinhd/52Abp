using MockSchoolManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MockSchoolManagement.DataRepositories
{
    public class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class
    {
        private readonly AppDbContext _appDbContext;
        public virtual DbSet<TEntity> Table => _appDbContext.Set<TEntity>();

        public RepositoryBase(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            Save();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entry in GetAll().Where(predicate))
            {
                Delete(entry);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entry in GetAll().Where(predicate))
            {
                await DeleteAsync(entry);
            }
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public TEntity Insert(TEntity entity)
        {
            var newEntity = Table.Add(entity).Entity;
            Save();

            return newEntity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            var newEntity = await Table.AddAsync(entity);
            await SaveAsync();

            return newEntity.Entity;
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _appDbContext.Entry(entity).State = EntityState.Modified;
            Save();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            _appDbContext.Entry(entity).State = EntityState.Modified;
            await SaveAsync();

            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Single();
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).SingleAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).FirstOrDefaultAsync();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _appDbContext.ChangeTracker.Entries()
                                .FirstOrDefault(entity => entity.Entity == entity);
            if (entity != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        protected void Save()
        {
            _appDbContext.SaveChanges();
        }

        protected async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
