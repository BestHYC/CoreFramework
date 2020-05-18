using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.ORM.Dapper
{

    public interface IExecuteBatch<T> where T : IEntity, new()
    {
        Int32 Execute(String sql, DynamicParameters parameters, Boolean transaction = false);
        Task<Int32> ExecuteAsync(String sql, DynamicParameters parameters, Boolean transaction = false);
        Int32 Execute(IBatch batch, Boolean transaction = false);
        Int32 Execute(IDbConnection connection, IBatch batch, IDbTransaction transaction);
        Task<Int32> ExecuteAsync(IDbConnection connection, IBatch batch, IDbTransaction transaction);
        Task<Int32> ExecuteAsync(IBatch batch, Boolean transaction = false);
        T Query(IBatch batch);
        Task<T> QueryAsync(IBatch batch);
        T Query(String sql, DynamicParameters parameters);
        Task<T> QueryAsync(String sql, DynamicParameters parameters);
        K Query<K>(IBatch batch);
        Task<K> QueryAsync<K>(IBatch batch);
        K Query<K>(String sql, DynamicParameters parameters);
        Task<K> QueryAsync<K>(String sql, DynamicParameters parameters);
        IEnumerable<T> QueryList(IBatch batch);
        Task<IEnumerable<T>> QueryListAsync(IBatch batch);
        IEnumerable<T> ExecuteReader(IBatch batch);
        Task<IEnumerable<T>> ExecuteReaderAsync(IBatch batch);
        IEnumerable<Tuple<T, K>> ExecuteReader<K>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new();
        Task<IEnumerable<Tuple<T, K>>> ExecuteReaderAsync<K>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new();
        IEnumerable<Tuple<T, K, P>> ExecuteReader<K, P>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new() where P : IEntity, new();
        Task<IEnumerable<Tuple<T, K, P>>> ExecuteReaderAsync<K, P>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new() where P : IEntity, new();
    }

    public class DapperExecuteBatch<T> : IExecuteBatch<T> where T : IEntity, new()
    {
        public Func<IDbConnection> Create { get; }
        public DapperExecuteBatch(Func<IDbConnection> create)
        {
            Create = create;
        }
        public Int32 Execute(String sql, DynamicParameters parameters, Boolean transaction = false)
        {
            if(SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            Int32 result = 0;
            using (var Connection = Create.Invoke())
            {
                if (transaction)
                {
                    IDbTransaction dbTransaction = null;
                    try
                    {
                        Connection.Open();
                        dbTransaction = Connection.BeginTransaction();
                        result = Connection.Execute(sql, parameters, dbTransaction, commandTimeout: 3600);
                        dbTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
                else
                {
                    result = Connection.Execute(sql, parameters, commandTimeout: 3600);
                }
                return result;
            }
        }
        public async Task<Int32> ExecuteAsync(String sql, DynamicParameters parameters, Boolean transaction = false)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            Int32 result = 0;
            using (var Connection = Create.Invoke())
            {
                if (transaction)
                {
                    IDbTransaction dbTransaction = null;
                    try
                    {
                        Connection.Open();
                        dbTransaction = Connection.BeginTransaction();
                        result = await Connection.ExecuteAsync(sql, parameters, dbTransaction, commandTimeout: 3600);
                        dbTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
                else
                {
                    result = await Connection.ExecuteAsync(sql, parameters, commandTimeout: 3600);
                }
                return result;
            }
        }
        public Int32 Execute(IBatch batch, Boolean transaction = false)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            Int32 result = 0;
            using (var Connection = Create.Invoke())
            {
                if (transaction)
                {
                    IDbTransaction dbTransaction = null;
                    try
                    {
                        Connection.Open();
                        dbTransaction = Connection.BeginTransaction();
                        result = Connection.Execute(batch.SqlBuilder, batch.DynamicParameters, dbTransaction, commandTimeout: 3600);
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
                else 
                {
                    result = Connection.Execute(batch.SqlBuilder, batch.DynamicParameters,  commandTimeout: 3600);
                }
            }
            return result;
        }
        public async Task<Int32> ExecuteAsync(IBatch batch, Boolean transaction = false)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            Int32 result = 0;
            using (var Connection = Create.Invoke())
            {
                if (transaction)
                {
                    IDbTransaction dbTransaction = null;
                    try
                    {
                        Connection.Open();
                        dbTransaction = Connection.BeginTransaction();
                        result = await Connection.ExecuteAsync(batch.SqlBuilder, batch.DynamicParameters, dbTransaction, commandTimeout: 3600);
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
                else
                {
                    result = await Connection.ExecuteAsync(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                }
            }
            return result;
        }
        /// <summary>
        /// 这个方法不对connction进行开启或者释放,注意,使用在外部使用using语句,若使用transation请手动open
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="batch"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Int32 Execute(IDbConnection connection, IBatch batch, IDbTransaction transaction)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            return connection.Execute(batch.SqlBuilder, batch.DynamicParameters, transaction, commandTimeout:3600);
        }
        /// <summary>
        /// 注意,此方法适合用用原生dapper处理的程序,手动open及close
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="batch"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<Int32> ExecuteAsync(IDbConnection connection, IBatch batch, IDbTransaction transaction)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            return await connection.ExecuteAsync(batch.SqlBuilder, batch.DynamicParameters, transaction, commandTimeout: 3600);
        }
        public T Query(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<T> list = Connection.Query<T>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                if(list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(T);
            }
        }
        public async Task<T> QueryAsync(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<T> list = await Connection.QueryAsync<T>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(T);
            }
        }
        public K Query<K>(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<K> list = Connection.Query<K>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(K);
            }
        }
        public async Task<K> QueryAsync<K>(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<K> list = await Connection.QueryAsync<K>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(K);
            }
        }
        public K Query<K>(String sql, DynamicParameters parameters)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<K> list = Connection.Query<K>(sql, parameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(K);
            }
        }
        public async Task<K> QueryAsync<K>(String sql, DynamicParameters parameters)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<K> list = await Connection.QueryAsync<K>(sql, parameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(K);
            }
        }
        public T Query(String sql, DynamicParameters parameters)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<T> list = Connection.Query<T>(sql, parameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(T);
            }
        }
        public async Task<T> QueryAsync(String sql, DynamicParameters parameters)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(sql, parameters);
            }
            using (var Connection = Create.Invoke())
            {
                IEnumerable<T> list = await Connection.QueryAsync<T>(sql, parameters, commandTimeout: 3600);
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default(T);
            }
        }
        public IEnumerable<T> QueryList(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                return Connection.Query<T>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
            }
        }
        public async Task<IEnumerable<T>> QueryListAsync(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            using (var Connection = Create.Invoke())
            {
                return await Connection.QueryAsync<T>(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
            }
        }
        public IEnumerable<T> ExecuteReader(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<T> entitybuffer = new List<T>(1024);
            using (var Connection = Create.Invoke())
            {
                var reader = Connection.ExecuteReader(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                var entityParser = reader.GetRowParser<T>();
                while (reader.Read())
                {
                    var entity = entityParser.Invoke(reader);
                    if (entity == null) break;
                    entitybuffer.Add(entity);
                }
            }
            return entitybuffer;
        }
        public async Task<IEnumerable<T>> ExecuteReaderAsync(IBatch batch)
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<T> entitybuffer = new List<T>(1024);
            using (var Connection = Create.Invoke())
            {
                var reader = await Connection.ExecuteReaderAsync(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                var entityParser = reader.GetRowParser<T>();
                while (reader.Read())
                {
                    var entity = entityParser.Invoke(reader);
                    if (entity == null) continue;
                    entitybuffer.Add(entity);
                }
            }
            return entitybuffer;
        }
        public IEnumerable<Tuple<T, K>> ExecuteReader<K>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new()
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<Tuple<T, K>> entitybuffer = new List<Tuple<T, K>>(128);
            using (var Connection = Create.Invoke())
            {
                var reader = Connection.ExecuteReader(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                while (reader.Read())
                {
                    T t = new T();
                    Type t1 = typeof(T);
                    K k = new K();
                    Type k1 = typeof(K);
                    for (Int32 i = 0; i < reader.FieldCount; i++)
                    {
                        Object obj = reader.GetValue(i);
                        if (obj != null)
                        {
                            ColumnRelevanceMapper column = mapper[i];
                            if (column.TableName == t1)
                            {
                                PropertyValueExpression<T>.SetValue(t, column.ColumnName, obj);
                            }
                            else
                            {
                                PropertyValueExpression<K>.SetValue(k, column.ColumnName, obj);
                            }
                        }
                    }
                    Tuple<T, K> tuple = new Tuple<T, K>(t, k);
                    entitybuffer.Add(tuple);
                }
            }
            return entitybuffer;
        }
        public async Task<IEnumerable<Tuple<T, K>>> ExecuteReaderAsync<K>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new()
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<Tuple<T, K>> entitybuffer = new List<Tuple<T, K>>(128);
            using (var Connection = Create.Invoke())
            {
                var reader = await Connection.ExecuteReaderAsync(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                while (reader.Read())
                {
                    T t = new T();
                    Type t1 = typeof(T);
                    K k = new K();
                    Type k1 = typeof(K);
                    for (Int32 i = 0; i < reader.FieldCount; i++)
                    {
                        Object obj = reader.GetValue(i);
                        if (obj != null)
                        {
                            ColumnRelevanceMapper column = mapper[i];
                            if (column.TableName == t1)
                            {
                                PropertyValueExpression<T>.SetValue(t, column.ColumnName, obj);
                            }
                            else
                            {
                                PropertyValueExpression<K>.SetValue(k, column.ColumnName, obj);
                            }
                        }
                    }
                    Tuple<T, K> tuple = new Tuple<T, K>(t, k);
                    entitybuffer.Add(tuple);
                }
            }
            return entitybuffer;
        }

        public IEnumerable<Tuple<T, K, P>> ExecuteReader<K, P>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new() where P : IEntity, new()
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<Tuple<T, K, P>> entitybuffer = new List<Tuple<T, K, P>>(128);
            using (var Connection = Create.Invoke())
            {
                var reader = Connection.ExecuteReader(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                while (reader.Read())
                {
                    T t = new T();
                    Type t1 = typeof(T);
                    K k = new K();
                    Type k1 = typeof(K);
                    P p = new P();
                    Type p1 = typeof(P);
                    for (Int32 i = 0; i < reader.FieldCount; i++)
                    {
                        Object obj = reader.GetValue(i);
                        if (obj != null)
                        {
                            ColumnRelevanceMapper column = mapper[i];
                            if (column.TableName == t1)
                            {
                                PropertyValueExpression<T>.SetValue(t, column.ColumnName, obj);
                            }
                            else if (column.TableName == k1)
                            {
                                PropertyValueExpression<K>.SetValue(k, column.ColumnName, obj);
                            }
                            else
                            {
                                PropertyValueExpression<P>.SetValue(p, column.ColumnName, obj);
                            }
                        }
                    }
                    Tuple<T, K, P> tuple = new Tuple<T, K, P>(t, k, p);
                    entitybuffer.Add(tuple);
                }
            }
            return entitybuffer;
        }
        public async Task<IEnumerable<Tuple<T, K, P>>> ExecuteReaderAsync<K, P>(IBatch batch, List<ColumnRelevanceMapper> mapper) where K : IEntity, new() where P : IEntity, new()
        {
            if (SqlExecuteExtensionAction.Instance.BatchAction != null)
            {
                SqlExecuteExtensionAction.Instance.AddContext(batch);
            }
            List<Tuple<T, K, P>> entitybuffer = new List<Tuple<T, K, P>>(128);
            using (var Connection = Create.Invoke())
            {
                var reader = await Connection.ExecuteReaderAsync(batch.SqlBuilder, batch.DynamicParameters, commandTimeout: 3600);
                while (reader.Read())
                {
                    T t = new T();
                    Type t1 = typeof(T);
                    K k = new K();
                    Type k1 = typeof(K);
                    P p = new P();
                    Type p1 = typeof(P);
                    for (Int32 i = 0; i < reader.FieldCount; i++)
                    {
                        Object obj = reader.GetValue(i);
                        if (obj != null)
                        {
                            ColumnRelevanceMapper column = mapper[i];
                            if (column.TableName == t1)
                            {
                                PropertyValueExpression<T>.SetValue(t, column.ColumnName, obj);
                            }
                            else if (column.TableName == k1)
                            {
                                PropertyValueExpression<K>.SetValue(k, column.ColumnName, obj);
                            }
                            else
                            {
                                PropertyValueExpression<P>.SetValue(p, column.ColumnName, obj);
                            }
                        }
                    }
                    Tuple<T, K, P> tuple = new Tuple<T, K, P>(t, k, p);
                    entitybuffer.Add(tuple);
                }
            }
            return entitybuffer;
        }
    }
}
