using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public override TEntity Insert(TEntity entity, Action<TEntity> action)
        {
            if (action != null) action.Invoke(entity);
            var newEntity = Table.Add(entity).Entity;
            _dbContext.SaveChanges();
            return newEntity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, Action<TEntity> action = null)
        {
            if (action != null) action.Invoke(entity);
            var entityEntry = await Table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public override void Insert(List<TEntity> entities, Action<List<TEntity>> action = null)
        {
            if (action != null) action.Invoke(entities);
            Table.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public override async Task InsertAsync(List<TEntity> entities, Action<List<TEntity>> action = null)
        {
            if (action != null) action.Invoke(entities);
            await Table.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public override TEntity Update(TEntity entity, Action<TEntity> action = null)
        {
            AttachIfNot(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            if (action != null) action.Invoke(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public override void Delete(TEntity entity, Action<TEntity> action = null)
        {
            if (entity != null)
            {
                entity.Isdelete = DBDefault.Delete;
                Update(entity, action);
            }
        }

        public override void Delete(TPrimaryKey id, Action<TEntity> action = null)
        {
            var entity = Get(id);
            Delete(entity, action);
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null)
        {
            var entities = GetAll(predicate);
            if (entities.Any())
            {
                entities.ForEach(entity =>
                {
                    Delete(entity, action);
                });
            }
        }

        public override void HardDelete(TEntity entity, Action<TEntity> action = null)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            if (action != null) action.Invoke(entity);
            _dbContext.SaveChanges();
        }

        public override void HardDelete(TPrimaryKey id, Action<TEntity> action = null)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                HardDelete(entity, action);
                return;
            }

            entity = Get(id);
            if (entity != null)
            {
                HardDelete(entity, action);
                return;
            }
        }

        public override void HardDelete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null)
        {
            var entities = Table.Where(predicate).ToList();
            if (entities.Any())
            {
                entities.ForEach(entity =>
                {
                    AttachIfNot(entity);
                    if (action != null) action.Invoke(entity);
                });
                Table.RemoveRange(entities);
                _dbContext.SaveChanges();
            }
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _dbContext.ChangeTracker.Entries().FirstOrDefault(ent => 
            {
                var item = ent.Entity as TEntity;
                if (item != null)
                {
                    return Object.Equals(item.Id, entity.Id);
                }
                return false;
            });
            
            if (entry != null)
            {
                _dbContext.Entry(entry.Entity).State = EntityState.Detached;
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


    }
}
