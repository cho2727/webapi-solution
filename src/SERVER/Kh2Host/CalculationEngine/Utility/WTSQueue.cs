using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationEngine.Utility
{
    public class WTSQueue<T> : IEnumerable<T>
    {
        #region 로컬변수

        private List<T>? queue = null;
        
        #endregion

        #region 생성자

        public WTSQueue()
        {
            queue = new List<T>();
        }

        public WTSQueue(int capacity)
        {
            queue = new List<T>(capacity);
        }

        #endregion

        #region Public 함수

        public void Add(T item)
        {
            queue.Add(item);
        }

        public void Clear()
        {
            queue.Clear();
        }

        public T? Dequeue()
        {
            if (queue.Count == 0)
                return default(T);

            T t = queue[0];
            queue.RemoveAt(0);

            return t;
        }

        #endregion

        #region Public 프로퍼티

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (Count == 0);
            }
        }

        #endregion

        #region 인덱서

        public T this[int index]
        {
            get
            {
                return queue[index];
            }
        }

        #endregion

        #region IEnumerable<T> 멤버

        public IEnumerator<T> GetEnumerator()
        {
            return new WTSQueueEnumerator<T>(queue);
        }

        #endregion

        #region IEnumerable 멤버

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new WTSQueueEnumerator<T>(queue);
        }
        #endregion
    }

    public class WTSQueueEnumerator<T> : IEnumerator<T>
    {
        #region 로컬 변수

        private List<T>? items = null;
        private int location;

        #endregion

        #region 생성자

        public WTSQueueEnumerator(List<T> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<T> Members

        public T Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return items[location];
                }
                else
                {
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        #endregion

        #region IDisposable 멤버

        public void Dispose()
        {

        }

        #endregion

        #region IEnumerator 멤버

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return (object)items[location];
                }
                else
                {
                    // we are outside the bounds					
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        public bool MoveNext()
        {
            location++;
            return (location < items.Count);
        }

        public void Reset()
        {
            location = -1;
        }

        #endregion
    }
}
