﻿namespace Problem03.ReversedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReversedList<T> : IAbstractList<T>
    {
        private const int DefaultCapacity = 4;

        private T[] items;

        public ReversedList()
            : this(DefaultCapacity) { }

        public ReversedList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            this.items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                this.ValidateIndex(index);
                return this.items[this.Count - 1 - index];
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
            this.GrowIfNecessary();
            this.items[this.Count++] = item;
        }

        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }


        public int IndexOf(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.items[this.Count - 1 - i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            this.ValidateIndex(index);
            this.GrowIfNecessary();

            var indexToInsertAt = this.Count - index;

            for (int i = this.Count; i > indexToInsertAt; i--)
            {
                this.items[i] = this.items[i - 1];
            }

            this.items[indexToInsertAt] = item;
            this.Count++;
        }

        public bool Remove(T item)
        {
            var indexToRemoveAt = this.IndexOf(item);

            if (indexToRemoveAt == -1)
            {
                return false;
            }

            this.RemoveAt(indexToRemoveAt);
            return true;
        }

        public void RemoveAt(int index)
        {
            this.ValidateIndex(index);

            var indexToRemoveAt = this.Count - 1 - index;

            for (int i = indexToRemoveAt; i < this.Count - 1; i++)
            {
                this.items[i] = this.items[i + 1];
            }
                
            this.items[this.Count - 1] = default;
            this.Count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                yield return this.items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
        }

        private void GrowIfNecessary()
        {
            if (this.Count == this.items.Length)
            {
                this.items = this.Grow();
            }
        }

        private T[] Grow()
        {
            var newArray = new T[this.Count * 2];
            Array.Copy(this.items, newArray, this.items.Length);
            return newArray;
        }
    }
}