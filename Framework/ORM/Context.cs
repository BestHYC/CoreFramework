using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.ORM
{
    public class DapperContext
    {              //连接字符串
        public static string connectionString = "";
        public static Func<IDbConnection> Connection;
    }
    public class DapperDbContext
    {
        public static DapperDbContext Instance = new DapperDbContext();
        public Dictionary<String, Func<IDbConnection>> Funcs { get; }
        private DapperDbContext()
        {
            Funcs = new Dictionary<String, Func<IDbConnection>>();
        }
        /// <summary>
        /// 如果有多个数据库操作的,请通过名称区分
        /// </summary>
        /// <param name="func"></param>
        /// <param name="connect"></param>
        /// <returns></returns>
        public DapperDbContext AddContext(Func<IDbConnection> func, ConnectTypeEnum connect = ConnectTypeEnum.None)
        {
            String name = connect.ToString();
            AddContext(func, name);
            return this;
        }
        public DapperDbContext AddContext(Func<IDbConnection> func, String name)
        {
            if (String.IsNullOrEmpty(name) || func == null)
            {
                throw new ArgumentException("不允许默认为空的字典项出现");
            }
            Funcs.Add(name, func);
            return this;
        }
    }
    public enum ConnectTypeEnum
    {
        None = 0, First = 1, Second = 2, Third = 3, Fourth = 4, Fifth = 5, Sixth = 6,
        Mysql = 7, Sqlserver = 8
    }
}
