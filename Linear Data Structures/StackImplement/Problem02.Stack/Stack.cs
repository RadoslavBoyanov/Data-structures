﻿using System.Collections;

namespace Problem02.Stack
{
    public class Stack<T> : IAbstractStack<T>
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

        private Node top;

        public Stack()
        {
            this.top = null;
            this.Count = 0;
        }

        public int Count { get; private set; }

        public void Push(T item)
        {
            Node newTop = new Node(item);
            newTop.Next = this.top;
            this.top = newTop;
            this.Count++;
        }

        public T Pop()
        {
           ValidateStackIfIsEmpty();

           T item = this.top.Element;
           this.top = this.top.Next;
           this.Count--;

           return item;
        }

        public T Peek()
        {
            this.ValidateStackIfIsEmpty();

            return this.top.Element;
        }

        public bool Contains(T item)
        {
            Node current = this.top;

            while (current != null)
            {
                if (current.Element.Equals(item))
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = this.top;

            while (current != null)
            {
                yield return current.Element;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void ValidateStackIfIsEmpty()
        {
            if (this.top == null)
            {
                throw new InvalidOperationException("Stack is empty!");
            }
        }
    }
}
