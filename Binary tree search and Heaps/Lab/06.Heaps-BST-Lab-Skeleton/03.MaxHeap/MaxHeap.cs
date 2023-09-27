namespace _03.MaxHeap
{
    using System;
    using System.Collections.Generic;


    public class MaxHeap<T> : IAbstractHeap<T> where T : IComparable<T>
    {
        private List<T> heap;

        public MaxHeap()
        {
            this.heap = new List<T>();
        }

        public int Size => this.heap.Count;

        public void Add(T element)
        {
            this.heap.Add(element);
            this.HeapifyUp(this.heap.Count - 1);
        }

        private void HeapifyUp(int index)
        {
            int parentIndex = this.GetParentIndex(index);
            while (index > 0 && IsGreater(index, parentIndex))
            {
                this.Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = this.GetParentIndex(index);
            }
        }

        private void Swap(int index, int parentIndex)
        {
            var temp = this.heap[index];
            this.heap[index] = this.heap[parentIndex];
            this.heap[parentIndex] = temp;
        }

        public T ExtractMax()
        {
            this.ValidateIfNotEmpty();

            T element = this.heap[0];
            this.Swap(0, this.Size - 1);
            this.heap.RemoveAt(this.Size - 1);
            this.HeapifyDown(0);
            return element;
        }

        private void HeapifyDown(int index)
        {
            var biggerChildIndex = this.GetBiggerChildIndex(index);

            while (IsIndexValid(biggerChildIndex) && this.IsGreater(biggerChildIndex, index))
            {
                this.Swap(biggerChildIndex, index);

                biggerChildIndex = this.GetBiggerChildIndex(index);
            }
        }

        private int GetBiggerChildIndex(int index)
        {
            var firstChildIndex = index * 2 + 1;
            var secondChildIndex = index * 2 + 2;

            if (secondChildIndex < this.heap.Count)
            {
                if (this.IsGreater(firstChildIndex, secondChildIndex))
                {
                    return firstChildIndex;
                }
                return secondChildIndex;
            }
            else if (firstChildIndex < this.heap.Count)
            {
                return firstChildIndex;
            }
            else
            {
                return -1;
            }
        }

        public T Peek()
        {
            this.ValidateIfNotEmpty();

            return this.heap[0];
        }


        private bool IsGreater(int index, int parentIndex)
        {
            return this.heap[index].CompareTo(this.heap[parentIndex]) > 0;
        }

        private int GetParentIndex(int index)
        {
            return (index - 1)/ 2;
        }

        private void ValidateIfNotEmpty()
        {
            if (this.heap.Count == 0)
            {
                throw new InvalidOperationException();
            }
        }

        private bool IsIndexValid(int index)
        {
            return index >= 0 && index < this.heap.Count;
        }
    }
}
