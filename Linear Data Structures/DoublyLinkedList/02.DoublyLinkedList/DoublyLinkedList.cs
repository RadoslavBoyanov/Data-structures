namespace Problem02.DoublyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DoublyLinkedList<T> : IAbstractLinkedList<T>
    {

        private class Node
        {
            public Node Next { get; set; }

            public Node Previous { get; set; }

            public T Value { get; set; }

            public Node(T value)
            {
                this.Value = value;
            }

        }

        private Node head;
        private Node tail;


        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            Node newNode = new Node(item);

            if (this.Count== 0)
            {
                this.AddingIfIsEmtyList(item);
            }
            else
            {
                this.head.Previous = newNode;
                newNode.Next = this.head;
                this.head = newNode;
            }

            this.Count++;
        }

        public void AddLast(T item)
        {
            Node newNode = new Node(item);
            if (this.Count == 0)
            {
                this.AddingIfIsEmtyList(item);
            }
            else
            {
                this.tail.Next = newNode;
                newNode.Previous = this.tail;
                this.tail = newNode;
            }
            this.Count++;
        }

        public T GetFirst()
        {
            this.Validator();

            return this.head.Value;
        }

        public T GetLast()
        {
            this.Validator();

            return this.tail.Value;
        }

        public T RemoveFirst()
        {
            this.Validator();

            Node current = this.head;

            if (this.Count == 1)
            {
                this.head = this.tail = null;
            }
            else
            {
                this.head = this.head.Next;
                this.head.Previous = null;
            }
            Count--;
            return current.Value;
            
        }

        public T RemoveLast()
        {
            this.Validator();

            Node current = this.tail;

            if (this.Count == 1)
            {
                this.tail = this.head = null;
            }
            else
            {
                this.tail = this.tail.Previous;
                this.tail.Next = null;
            }
            Count--;

            return current.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = this.head;
            while (current != null)
            {
                yield return current.Value; 
                current = current.Next;     
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void Validator()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Dobly Linked List is empty!");
            }
        }

        private void AddingIfIsEmtyList(T item)
        {
            Node newNode = new Node(item);
            this.head = newNode;
            this.tail = newNode;
        }
    }
}
