using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework
{
    public static class TaskExtension
    {
        /// <summary>
        /// 指向非泛型的TaskCompletionSource类
        /// </summary>
        private struct Void { }
        /// <summary>
        /// 取消IO操作时候可能会发生竞态条件,决定是丢弃还是实用数据
        /// 通过扩展Task<TResult>来实现
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="originalTask"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> originalTask, CancellationToken ct)
        {
            //创建在Cancellationsourcetask被取消时完成的一个Task
            var cancelTask = new TaskCompletionSource<Void>();
            //一旦CancellationToken被取消,就完成Task
            using (ct.Register(t => ((TaskCompletionSource<Void>)t).TrySetResult(new Void()), cancelTask))
            {
                //创建在原始Task或CancellationToken Task完成时返回一个Task
                Task any = await Task.WhenAny(originalTask, cancelTask.Task);
                //任何Task因为CancellationToken 完成,就抛出OperationCanceledException
                if (any == cancelTask.Task) ct.ThrowIfCancellationRequested();
            }
            //等待原始任务(以同步方式);若任务失败,等待它将抛出第一个内部异常
            //而不是抛出AggregateException
            return await originalTask;
        }
    }
}
