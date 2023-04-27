using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Problem01.List
{
    public class List<T> : IAbstractList<T>
    {
        private const int DEFAULT_CAPACITY = 4;
        private T[] items;

        public List()
            : this(DEFAULT_CAPACITY)
        {
        }

        public List(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                this.ValidateIndex(index);
                return this.items[index];
            }
            set
            {
                this.ValidateIndex(index);
                this.items[index] = value;
            }
        }

        public int Count { get; private set; }

        public void Add(T item)
        {
            IfNeedToGrow();
            this.items[this.Count++] = item;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (item.Equals(this.items[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.items[i];
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            ValidateIndex(index);
            IfNeedToGrow();

            for (int i = this.Count; i > index; i--)
            {
                this.items[i] = this.items[i - 1];
            }
            this.items[index] = item;
            this.Count++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);
            for (int i = index; i < this.Count; i++)
            {
                this.items[i] = this.items[i + 1];
            }

            this.items[this.Count - 1] = default;
            this.Count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private T[] Grow()
        {
            T[] newItems = new T[this.Count * 2];

            for (int i = 0; i < this.items.Length; i++)
            {
                newItems[i] = this.items[i];
            }

            return newItems;
        }

        private void IfNeedToGrow()
        {
            if (this.Count == this.items.Length)
                this.items = this.Grow();
        }

        private void ValidateIndex(int index)
        {
            if (index < 0|| index >= this.Count)
                throw new IndexOutOfRangeException("Index is out of range!");
        }
    }
}
