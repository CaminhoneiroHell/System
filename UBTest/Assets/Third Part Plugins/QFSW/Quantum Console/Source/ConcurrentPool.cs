using System.Collections.Concurrent;
using System.Linq;

namespace QFSW.QC
{
    public class ConcurrentPool<T> where T : new()
    {
        private ConcurrentStack<T> _objs;

        public ConcurrentPool()
        {
            _objs = new ConcurrentStack<T>();
        }

        public ConcurrentPool(int objCount)
        {
            _objs = new ConcurrentStack<T>(Enumerable.Repeat(new T(), objCount));
        }

        public T GetObject()
        {
            if (_objs.TryPop(out T obj))
            {
                return obj;
            }
            else
            {
                return new T();
            }
        }

        public void Release(T obj)
        {
            _objs.Push(obj);
        }
    }
}