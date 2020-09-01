using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ORM.EntityFramework
{
    public class Repository<TEntity>
             : Repository<TEntity, int>
             where TEntity : class, IEntity
    {
        public Repository(DbContext dbDbContext) : base(dbDbContext)
        {
        }
    }

    public class Repository<TEntity, TPrimaryKey>
        : RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly DbContext _dbContext;
        public virtual DbSet<TEntity> Table => _dbContext.Set<TEntity>();
        public Repository(DbContext context)
        {
            _dbContext = context;
        }

        public override IQueryable<TEntity> Query()
        {
            return Table.AsQueryable().AsNoTracking();
        }

        public override IQueryable<TEntity> QueryNoTracking()
        {
            return Table.AsQueryable().AsNoTracking();
        }

        public override TEntity Insert(TEntity entity)
        {
            var newEntity = Table.Add(entity).Entity;
            _dbContext.SaveChanges();
            return newEntity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await Table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public override void Insert(List<TEntity> entities)
        {
            Table.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public override async Task InsertAsync(List<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return entity;
        }
        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            if (entity != null)
            {
                entity.Isdelete = DBDefault.Delete;
                Update(entity);
            }
        }
        public override async Task DeleteAsync(TEntity entity)
        {
            if (entity != null)
            {
                entity.Isdelete = DBDefault.Delete;
                await UpdateAsync(entity);
            }
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = Get(id);
            Delete(entity);
        }
        public override async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = GetAll(predicate);
            if (entities.Any())
            {
                foreach (var item in entities)
                {
                    Delete(item);
                }
            }
        }
        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetAllAsync(predicate);
            if (entities.Any())
            {
                foreach(var item in entities)
                {
                    await DeleteAsync(item);
                }
            }
        }
        public override void HardDelete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            _dbContext.SaveChanges();
        }
        public override async Task HardDeleteAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public override void HardDelete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity == null)
            {
                entity = Get(id);
            }
            if (entity != null)
            {
                HardDelete(entity);
            }
        }
        public override async Task HardDeleteAsync(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity == null)
            {
                entity = await GetAsync(id);
            }
            if (entity != null)
            {
                await HardDeleteAsync(entity);
            }
        }

        public override void HardDelete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return;
            var entities = Table.Where(predicate).ToList();
            if (entities.Any())
            {
                entities.ForEach(entity =>
                {
                    AttachIfNot(entity);
                });
                Table.RemoveRange(entities);
                _dbContext.SaveChanges();
            }
        }
        public override async Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return;
            var entities = await Table.Where(predicate).ToListAsync();
            if (entities.Any())
            {
                entities.ForEach(entity =>
                {
                    AttachIfNot(entity);
                });
                Table.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
        }
        protected virtual void AttachIfNot(TEntity entity)
        {
            if (entity == null) return;
            var entry = GetFromChangeTrackerOrNull(entity.Id);
            if (entry != null)
            {
                _dbContext.Entry(entry).State = EntityState.Detached;
            }
            Table.Attach(entity);
        }
        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = _dbContext.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, ((TEntity)ent.Entity).Id)
                );

            return entry?.Entity as TEntity;
        }
        public async Task<IEnumerable<T>> Execute<T>(string sql, object obj)
        {
            IEnumerable<T> list = null;
            using (IDbConnection connection = DapperContext.Connection())
            {
                list = await connection.QueryAsync<T>(sql, obj);
            }
            return list;
        }
    }
}
