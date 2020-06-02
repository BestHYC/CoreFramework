using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ORM.EntityFramework
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Query 注意,所有的query操作都默认是Notracking操作,如果想跟踪,请自己实现
        public abstract IQueryable<TEntity> Query();

        public abstract IQueryable<TEntity> QueryNoTracking();

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Where(predicate);
        }

        public virtual IQueryable<TEntity> QueryNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryNoTracking().Where(predicate);
        }

        public virtual List<TEntity> GetAll()
        {
            return QueryNoTracking().ToList();
        }

        public virtual Task<List<TEntity>> GetAllAsync()
        {
            return QueryNoTracking().ToListAsync();
        }

        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryNoTracking().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryNoTracking().Where(predicate).ToListAsync();
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            return FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public virtual TEntity FirstOrDefault()
        {
            return QueryNoTracking().FirstOrDefault();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync()
        {
            return QueryNoTracking().FirstOrDefaultAsync();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryNoTracking().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return QueryNoTracking().FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region Insert
        public abstract TEntity Insert(TEntity entity, Action<TEntity> action = null);

        public abstract Task<TEntity> InsertAsync(TEntity entity, Action<TEntity> action = null);

        public abstract void Insert(List<TEntity> entities, Action<List<TEntity>> action = null);

        public abstract Task InsertAsync(List<TEntity> entities, Action<List<TEntity>> action = null);
        #endregion

        #region 修改
        public abstract TEntity Update(TEntity entity, Action<TEntity> action = null);

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, Action<TEntity> action = null)
        {
            return await Task.FromResult(Update(entity, action));
        }
        #endregion

        #region Delete
        public abstract void Delete(TEntity entity, Action<TEntity> action= null);

        public virtual async Task DeleteAsync(TEntity entity, Action<TEntity> action = null)
        {
            Delete(entity, action);
            await Task.CompletedTask;
        }

        public abstract void Delete(TPrimaryKey id, Action<TEntity> action = null);

        public virtual Task DeleteAsync(TPrimaryKey id, Action<TEntity> action = null)
        {
            Delete(id, action);
            return Task.CompletedTask;
        }

        public abstract void Delete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null)
        {
            Delete(predicate, action);
            return Task.CompletedTask;
        }
        #endregion

        #region HardDelete
        public abstract void HardDelete(TEntity entity, Action<TEntity> action = null);

        public virtual async Task HardDeleteAsync(TEntity entity, Action<TEntity> action = null)
        {
            HardDelete(entity,action);
            await Task.CompletedTask;
        }

        public abstract void HardDelete(TPrimaryKey id, Action<TEntity> action = null);

        public virtual async Task HardDeleteAsync(TPrimaryKey id, Action<TEntity> action = null)
        {
            HardDelete(id, action);
            await Task.CompletedTask;
        }

        public abstract void HardDelete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);

        public virtual async Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null)
        {
            HardDelete(predicate, action);
            await Task.CompletedTask;
        }
        #endregion

        #region Aggregate
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().AnyAsync(predicate);
        }

        public virtual int Count()
        {
            return Query().Count();
        }

        public virtual async Task<int> CountAsync()
        {
            return await Query().CountAsync();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Where(predicate).Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().CountAsync(predicate);
        }

        public virtual long LongCount()
        {
            return Query().LongCount();
        }

        public virtual async Task<long> LongCountAsync()
        {
            return await Query().LongCountAsync();
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Where(predicate).LongCount();
        }

        public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().LongCountAsync(predicate);
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
        #endregion
    }
}
