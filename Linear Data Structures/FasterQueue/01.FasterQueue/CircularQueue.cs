namespace Problem01.CircularQueue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CircularQueue<T> : IAbstractQueue<T>
    {
        public int DEFAULT_CAPACITY = 4;
        private T[] items;
        private int startIndex;
        private int endIndex;

        public CircularQueue() 
        {
            this.items = new T[DEFAULT_CAPACITY];
        }

        public int Count { get; private set; }

        public T Dequeue()
        {
            
                this.CheckIfCircularQueueIsEmpty();
           
            T item = this.items[startIndex];
            this.startIndex = (this.startIndex + 1) % this.items.Length;
            this.Count--;
            return item;
        }

        public void Enqueue(T item)
        {
            if (this.Count >= this.items.Length)
            {
                this.GrowQueue();
            }

            this.items[this.endIndex] = item;
            this.endIndex = (this.endIndex + 1) % this.items.Length;
            this.Count++;
        }

        public T Peek()
        {
            this.CheckIfCircularQueueIsEmpty();

            return this.items[this.startIndex];
        }

        public T[] ToArray()
        {
            var newArray = new T[this.Count];

            for (int currentIndex = 0; currentIndex < this.Count; currentIndex++)
            {
                newArray[currentIndex] = this.items[(this.startIndex + currentIndex) % this.items.Length];
            }

            return newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int currentIndex = 0; currentIndex < this.Count; currentIndex++)
            {
                yield return this.items[(this.startIndex + currentIndex) % this.items.Length];

            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void CheckIfCircularQueueIsEmpty()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Circular queue is empty!");
            }
        }

        private void GrowQueue()
        {
            var newArray = new T[this.items.Length * 2];
            this.startIndex = 0;
            this.endIndex = this.Count;

            for (int currentIndex = 0; currentIndex < this.Count; currentIndex++)
            {
                newArray[currentIndex] = this.items[(this.startIndex + currentIndex) % this.items.Length];
            }

            this.items = newArray;
        }
    }

}
