using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC
{
    public class Pool<T> where T : new()
    {
        private Stack<T> _objs;

        public Pool()
        {
            _objs = new Stack<T>();
        }

        public Pool(int objCount)
        {
            _objs = new Stack<T>(Enumerable.Repeat(new T(), objCount));
        }

        public T GetObject()
        {
            if (_objs.Count > 0)
            {
                return _objs.Pop();
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