using System;
using System.Data;

namespace Framework.ORM.Dapper
{
    public interface ISqlBuilder<T> where T: IEntity,new()
    {
        Query<T> CreateQueryService();
        Renewal<T> CreateRenewalService();
        IExecuteBatch<T> CreateDapper();
    }
    public interface ISqlBuilderFactory<T> where T : IEntity,new()
    {
        ISqlBuilder<T> CreateBuilder();
    }
    public class SqlSererBuilder<T> : ISqlBuilder<T> where T:IEntity,new()
    {
        private Func<IDbConnection> Connection;
        public SqlSererBuilder(Func<IDbConnection> conn)
        {
            Connection = conn;
        }
        public IExecuteBatch<T> CreateDapper()
        {
            return new DapperExecuteBatch<T>(Connection);
        }

        public Query<T> CreateQueryService()
        {
            return new SqlServerQuery<T>();
        }

        public Renewal<T> CreateRenewalService()
        {
            return new SqlServerRenewal<T>();
        }
    }

    public interface IConnectionCreate
    {
        IDbConnection CreateConnection();
    }


    public class MysqlBuilder<T> : ISqlBuilder<T> where T : IEntity, new()
    {
        private Func<IDbConnection> Connection;
        public MysqlBuilder(Func<IDbConnection> conn)
        {
            Connection = conn;
        }
        public IExecuteBatch<T> CreateDapper()
        {
            return new DapperExecuteBatch<T>(Connection);
        }

        public Query<T> CreateQueryService()
        {
            return new MysqlQuery<T>();
        }

        public Renewal<T> CreateRenewalService()
        {
            return new MysqlRenewal<T>();
        }
    }
    public class SqlBuilderFactory<T> : ISqlBuilderFactory<T> where T : IEntity, new()
    {
        private Func<IDbConnection> m_connection;
        private static String m_type;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlEnum">mysql种类</param>
        /// <param name="connString">字符串连接</param>
        public SqlBuilderFactory(Func<IDbConnection> connection)
        {
            m_connection = connection;
            if(m_type == null)
            {
                using (var i = connection.Invoke())
                {
                    m_type = i.GetType().Name;
                }
            }
        }
        /// <summary>
        /// 创建SqlBuilder
        /// </summary>
        /// <returns></returns>
        public ISqlBuilder<T> CreateBuilder()
        {
            switch (m_type)
            {
                case "SqlConnection":
                    return new SqlSererBuilder<T>(m_connection);
                case "MySqlConnection":
                    return new MysqlBuilder<T>(m_connection);
                default: return new SqlSererBuilder<T>(m_connection);
            }
        }
    }
}
