using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 使用简单锁性质的hashset集合类
    /// 因为HashSet的高速,没必要使用事件锁,浪费大量性能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockHashSet<T> : IEnumerable<T>
    {
        private readonly HashSet<T> m_set = new HashSet<T>();
        private SimpleSpinLock m_lock = new SimpleSpinLock();
        public void Add(T t)
        {
            m_lock.Enter();
            m_set.Add(t);
            m_lock.Leave();
        }
        public Boolean Remove(T t)
        {
            m_lock.Enter();
            var a = m_set.Remove(t);
            m_lock.Leave();
            return a;
        }
        public Int32 Count()
        {
            m_lock.Enter();
            Int32 i = m_set.Count();
            m_lock.Leave();
            return i;
        }
        public Boolean Contains(T t)
        {
            m_lock.Enter();
            Boolean isContains = m_set.Contains(t);
            m_lock.Leave();
            return isContains;
        }
        public void Clear()
        {
            m_lock.Enter();
            m_set.Clear();
            m_lock.Leave();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return m_set as IEnumerator<T>;
        }

        public IEnumerator GetEnumerator()
        {
            foreach(var item in m_set)
            {
                yield return item;
            }
        }
    }
}
