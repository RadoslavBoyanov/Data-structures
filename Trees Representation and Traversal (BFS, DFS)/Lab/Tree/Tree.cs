namespace Tree
{
    using System;
    using System.Collections.Generic;

    public class Tree<T> : IAbstractTree<T>
    {
        private List<Tree<T>> children;
        private Tree<T> parent;
        private T value;


        public Tree(T value)
        {
            this.value = value;
            this.children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            foreach (var child in children)
            {
                child.parent = this;
                this.children.Add(child);
            }
        }

        public void AddChild(T parentKey, Tree<T> child)
        {
            var seachNodeForDelete = this.FindBfs(parentKey);

            this.CheckIfNodeIsEmpty(seachNodeForDelete);

            seachNodeForDelete.children.Add(child);
        }

        public IEnumerable<T> OrderBfs()
        {
            var result = new List<T>();
            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var subtree = queue.Dequeue();
                result.Add(subtree.value);


                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }
        //using Stack
        //public IEnumerable<T> OrderDfs()
        //{
        //    var result = new Stack<T>();
        //    var stack = new Stack<Tree<T>>();
        //    stack.Push(this);

        //    while(stack.Count > 0)
        //    {
        //        var node = stack.Pop();

        //        foreach (var child in node.children)
        //        {
        //            stack.Push(child);
        //        }
        //        result.Push(node.value);
        //    }
        //    return result;
        //}

        //using Recursive
        public IEnumerable<T> OrderDfs()
        {
            var result = new List<T>();
            this.Dfs(this, result);
            return result;
        }

        public void RemoveNode(T nodeKey)
        {
            var searchedNode = this.FindBfs(nodeKey);

            this.CheckIfNodeIsEmpty(searchedNode);

            foreach (var child in searchedNode.children)
            {
                child.parent = null;
            }

            searchedNode.children.Clear();

            var searchedParent = searchedNode.parent;
            if (searchedParent == null)
            {
                throw new ArgumentException();
            }
            else
            {
                searchedParent.children.Remove(searchedNode);
            }

            searchedNode.value = default(T);
        }

        public void Swap(T firstKey, T secondKey)
        {
            var firstNode = this.FindBfs(firstKey);
            var secondNode = this.FindBfs(secondKey);

            this.CheckIfNodeIsEmpty(firstNode);
            this.CheckIfNodeIsEmpty(secondNode);

            var firstParent = firstNode.parent;
            var secondParent = secondNode.parent;

            if (firstParent is null || secondParent is null)
            {
                throw new ArgumentException("Some of the parents is null");
            }

            int indexOfFirtsParent = firstParent.children.IndexOf(firstNode);
            int indexOfSecondParent = secondParent.children.IndexOf(secondNode);

            firstParent.children[indexOfFirtsParent] = secondNode;
            secondParent.children[indexOfSecondParent] = firstNode;

        }

        //recursive method for DFS
        private void Dfs(Tree<T> tree, List<T> result)
        {
            foreach (var child in tree.children)
            {
                this.Dfs(child, result);
            }
            result.Add(tree.value);
        }

        //Find node with BFS method
        private Tree<T> FindBfs(T parentKey)
        {
            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var subtree = queue.Dequeue();

                if (subtree.value.Equals(parentKey))
                {
                    return subtree;
                }

                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

     
        private void CheckIfNodeIsEmpty(Tree<T> searchedNode)
        {
            if (searchedNode is null)
            {
                throw new ArgumentNullException("Node is not found!");
            }
        }
    }
}
