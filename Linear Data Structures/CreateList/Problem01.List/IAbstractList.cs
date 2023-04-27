using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem01.List
{
    public interface IAbstractList<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index] { get; set; }

        void Add(T item);

        void Insert(int index, T item);

        bool Contains(T item);

        int IndexOf(T item);

        bool Remove(T item);

        void RemoveAt(int index);
    }
}
