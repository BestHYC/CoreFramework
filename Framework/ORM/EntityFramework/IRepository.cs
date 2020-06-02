using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ORM.EntityFramework
{
    public interface IRepository<TEntity> : IRepository<TEntity, Int32>
            where TEntity : class, IEntity
    {
    }

    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Query

        /// <summary>
        /// 查询
        /// </summary>
        IQueryable<TEntity> Query();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询不跟踪实体变化
        /// </summary>
        IQueryable<TEntity> QueryNoTracking();
        IQueryable<TEntity> QueryNoTracking(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取
        /// </summary>
        TEntity Get(TPrimaryKey id);
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// 获取所有
        /// </summary>
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取第一个
        /// </summary>
        TEntity FirstOrDefault();
        Task<TEntity> FirstOrDefaultAsync();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Insert

        /// <summary>
        /// 新增
        /// </summary>
        TEntity Insert(TEntity entity, Action<TEntity> action = null);
        Task<TEntity> InsertAsync(TEntity entity, Action<TEntity> action = null);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        void Insert(List<TEntity> entities, Action<List<TEntity>> action = null);
        Task InsertAsync(List<TEntity> entities, Action<List<TEntity>> action = null);

        #endregion Insert

        #region Update

        /// <summary>
        /// 更新
        /// </summary>
        TEntity Update(TEntity entity, Action<TEntity> action = null);
        Task<TEntity> UpdateAsync(TEntity entity, Action<TEntity> action = null);

        #endregion Update

        #region Delete

        /// <summary>
        /// 逻辑删除，标记IsDelete = 1
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity, Action<TEntity> action = null);
        Task DeleteAsync(TEntity entity, Action<TEntity> action = null);
        void Delete(TPrimaryKey id, Action<TEntity> action = null);
        Task DeleteAsync(TPrimaryKey id, Action<TEntity> action = null);
        void Delete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);

        #endregion

        #region HardDelete

        /// <summary>
        /// 物理删除，从数据库中移除
        /// </summary>
        /// <param name="entity"></param>
        void HardDelete(TEntity entity, Action<TEntity> action = null);
        Task HardDeleteAsync(TEntity entity, Action<TEntity> action = null);
        void HardDelete(TPrimaryKey id, Action<TEntity> action = null);
        Task HardDeleteAsync(TPrimaryKey id, Action<TEntity> action = null);
        void HardDelete(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);
        Task HardDeleteAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> action = null);

        #endregion

        #region Aggregate

        /// <summary>
        /// 聚合操作
        /// </summary>
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        long LongCount();
        Task<long> LongCountAsync();
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion
    }
}
