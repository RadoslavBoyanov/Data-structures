namespace Problem03.Queue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Queue<T> : IAbstractQueue<T>
    {
        private class Node
        {
            public T Element { get; set; }
            public Node Next { get; set; }

            public Node(T element, Node next)
            {
                this.Element = element;
                this.Next = next;
            }

            public Node(T element)
            {
                this.Element = element;
            }
        }

        private Node head;

        public Queue()
        {
            this.head = null;
            this.Count = 0;
        }

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            Node currentNode = this.head;
            while (currentNode != null)
            {
                if (currentNode.Element.Equals(item))
                {
                    return true;
                }
                currentNode = currentNode.Next;
            }
            return false;
        }

        public T Dequeue()
        {
            EnsureIfNotEmpty();
            var current = this.head.Element;
            this.head = this.head.Next;
            this.Count--;

            return current;
        }

        public void Enqueue(T item)
        {
            var newNode = new Node(item);
            Node current = this.head;

            if (current is null)
                this.head = newNode;
            else
            {
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            this.Count++;
        }

        public T Peek()
        {
            EnsureIfNotEmpty();

            return this.head.Element;
        }

        public IEnumerator<T> GetEnumerator()
        {
           Node current = this.head;

           while (current != null)
           {
               yield return current.Element;
               current = current.Next;
           }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void EnsureIfNotEmpty()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty!");
            }
        }
    }
}