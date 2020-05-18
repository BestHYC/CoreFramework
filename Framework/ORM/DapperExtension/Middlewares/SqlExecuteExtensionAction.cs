using Dapper;
using Framework.ORM.Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.ORM.Dapper
{
    /// <summary>
    /// 将执行的sql显示
    /// </summary>
    public class SqlExecuteExtensionAction
    {
        public Action<String, DynamicParameters> BatchAction { get; set; }
        public static SqlExecuteExtensionAction Instance = new SqlExecuteExtensionAction();
        public ConcurrentQueue<Tuple<String, DynamicParameters>> Tuples { get; }
        private SqlExecuteExtensionAction() 
        {
            Tuples = new ConcurrentQueue<Tuple<String, DynamicParameters>>();
        }
        public void AddContext(IBatch batch)
        {
            Task.Run(() =>
            {
                Tuple<String, DynamicParameters> tuple = new Tuple<String, DynamicParameters>(batch.SqlBuilder, batch.DynamicParameters);
                Tuples.Enqueue(tuple);
                End();
            });
        }
        public void AddContext(String sql, DynamicParameters dynamic)
        {
            Task.Run(() =>
            {
                Tuple<String, DynamicParameters> tuple = new Tuple<String, DynamicParameters>(sql, dynamic);
                Tuples.Enqueue(tuple);
                End();
            });
        }

        public void End()
        {
            if(BatchAction != null)
            {
                Tuple<String, DynamicParameters> tuple = null;
                if (Tuples.TryDequeue(out tuple))
                {
                    BatchAction.Invoke(tuple.Item1, tuple.Item2);
                }
            }
        }
    }
}
