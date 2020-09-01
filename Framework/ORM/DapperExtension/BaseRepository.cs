using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ORM.Dapper
{ 
    public abstract class BaseRepository<T> where T : IEntity, new()
    {
        protected ISqlBuilder<T> m_builder { get; }
        protected Query<T> m_query { get { return m_builder.CreateQueryService(); } }
        protected Renewal<T> m_renewal { get { return m_builder.CreateRenewalService(); } }
        protected IExecuteBatch<T> m_executeBatch { get { return m_builder.CreateDapper(); } }
        public BaseRepository(ConnectTypeEnum sqlTypeEnum = ConnectTypeEnum.None):this(sqlTypeEnum.ToString())
        {
            
        }
        public BaseRepository(String connecttype)
        {
            if (!DapperDbContext.Instance.Funcs.ContainsKey(connecttype))
            {
                throw new Exception($"{typeof(T)}的Repository中找不到可连接数据");
            }
            m_builder = new SqlBuilderFactory<T>(DapperDbContext.Instance.Funcs[connecttype]).CreateBuilder();
        }
        /// <summary>
        /// 根据主键查询数据
        /// </summary>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public T Get(Object pkValue)
        {
            Type t = typeof(T);
            String name = EntityTableMapper.GetPkColumn(t);
            Expression<Func<T,Boolean>> expression = PropertyValueExpression<T>.BuildExpression(name, pkValue);
            return m_executeBatch.Query(m_query.Select().Where(expression).End());
        }
        public async Task<T> GetAsync(Object pkValue)
        {
            Type t = typeof(T);
            String name = EntityTableMapper.GetPkColumn(t);
            Expression<Func<T, Boolean>> expression = PropertyValueExpression<T>.BuildExpression(name, pkValue);
            return await m_executeBatch.QueryAsync(m_query.Select().Where(expression).End());
        }
        /// <summary>
        /// 根据刷选条件查询第一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, Boolean>> query)
        {
            return m_executeBatch.Query(m_query.Select().Where(query).End());
        }
        public async Task<T> GetAsync(Expression<Func<T, Boolean>> query)
        {
            return await m_executeBatch.QueryAsync(m_query.Select().Where(query).End());
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> QueryAll()
        {
            return m_executeBatch.QueryList(m_query.Select().End());
        }
        public async Task<IEnumerable<T>> QueryAllAsync()
        {
            return await m_executeBatch.QueryListAsync(m_query.Select().End());
        }
        public IEnumerable<T> QueryAllReader()
        {
            return m_executeBatch.ExecuteReader(m_query.Select().End());
        }
        public async Task<IEnumerable<T>> QueryAllReaderAsync()
        {
            return await m_executeBatch.ExecuteReaderAsync(m_query.Select().End());
        }
        /// <summary>
        /// 根据条件查询列表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList(Expression<Func<T, Boolean>> query)
        {
            return m_executeBatch.QueryList(m_query.Select().Where(query).End());
        }
        public async Task<IEnumerable<T>> QueryListAsync(Expression<Func<T, Boolean>> query)
        {
            return await m_executeBatch.QueryListAsync(m_query.Select().Where(query).End());
        }
        public IEnumerable<T> QueryListReader(Expression<Func<T, Boolean>> query)
        {
            return m_executeBatch.ExecuteReader(m_query.Select().Where(query).End());
        }
        public async Task<IEnumerable<T>> QueryListReaderAsync(Expression<Func<T, Boolean>> query)
        {
            return await m_executeBatch.ExecuteReaderAsync(m_query.Select().Where(query).End());
        }
        /// <summary>
        /// 根据id修改数据,如果没主键则报错,注意主键无法修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int32 Update(T model)
        {
            return m_executeBatch.Execute(m_renewal.Update(model).End());
        }
        public async Task<Int32> UpdateAsync(T model)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Update(model).End());
        }
        /// <summary>
        /// 需要更新的字段修改数据,注意主键无法修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="select">需要更新的字段</param>
        /// <returns></returns>
        public Int32 Update(T model, Expression<Func<T, Object>> select)
        {
            return m_executeBatch.Execute(m_renewal.Update(model, select).End());
        }
        public async Task<Int32> UpdateAsync(T model, Expression<Func<T, Object>> select)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Update(model, select).End());
        }
        /// <summary>
        /// 根据条件修改字段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public  Int32 Update(T model, Expression<Func<T, Boolean>> where)
        {
            return m_executeBatch.Execute(m_renewal.Update(model).Where(where).End());
        }
        public async Task<Int32> UpdateAsync(T model, Expression<Func<T, Boolean>> where)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Update(model).Where(where).End());
        }
        /// <summary>
        /// 根据条件修改字段 比如 只修改那么 (model, item=>new{item.name}, item=>item.id==model.id)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="select">需要更新的字段</param>
        /// <param name="where">需要更新的条件</param>
        /// <returns></returns>
        public Int32 Update(T model, Expression<Func<T, Object>> select, Expression<Func<T, Boolean>> where)
        {
            return m_executeBatch.Execute(m_renewal.Update(model, select).Where(where).End());
        }
        public async Task<Int32> UpdateAsync(T model, Expression<Func<T, Object>> select, Expression<Func<T, Boolean>> where)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Update(model, select).Where(where).End());
        }
        public Int32 Count(Expression<Func<T, Boolean>> where = null)
        {
            if(where == null)
            {
                where = item => 1 == 1;
            }
            return m_executeBatch.Query<Int32>(m_query.Count(where).End());
        }
        public async Task<Int32> CountAsync(Expression<Func<T, Boolean>> where = null)
        {
            if (where == null)
            {
                where = item => 1 == 1;
            }
            return await m_executeBatch.QueryAsync<Int32>(m_query.Count(where).End());
        }
        /// <summary>
        /// 插入数据,注意自增键无法新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Int32 Insert(T model)
        {
            return m_executeBatch.Execute(m_renewal.Insert(model).End());
        }
        public async Task<Int32> InsertAsync(T model)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Insert(model).End());
        }
        /// <summary>
        /// 插入数据,注意,主键无法插入
        /// </summary>
        /// <param name="model">传入的实体</param>
        /// <param name="select">需要插入的字段</param>
        /// <returns></returns>
        public Int32 Insert(T model, Expression<Func<T, Object>> select)
        {
            return m_executeBatch.Execute(m_renewal.Insert(model, select).End());
        }
        public async Task<Int32> InsertAsync(T model, Expression<Func<T, Object>> select)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Insert(model, select).End());
        }
        /// <summary>
        /// 根据id删除,如果没有主键 则报错
        /// </summary>
        /// <param name="model">传入的实体</param>
        /// <returns></returns>
        public Int32 Delete(T model)
        {
            return m_executeBatch.Execute(m_renewal.Delete(model).End());
        }
        public async Task<Int32> DeleteAsync(T model)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Delete(model).End());
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Int32 Delete(Expression<Func<T, Boolean>> where)
        {
            return m_executeBatch.Execute(m_renewal.Delete(where).End());
        }
        public async Task<Int32> DeleteAsync(Expression<Func<T, Boolean>> where)
        {
            return await m_executeBatch.ExecuteAsync(m_renewal.Delete(where).End());
        }
    }
}
