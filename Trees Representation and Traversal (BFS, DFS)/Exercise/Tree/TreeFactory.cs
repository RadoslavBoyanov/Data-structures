namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TreeFactory
    {
        private Dictionary<int, IntegerTree> nodesByKey;

        public TreeFactory()
        {
            this.nodesByKey = new Dictionary<int, IntegerTree>();
        }

        public IntegerTree CreateTreeFromStrings(string[] input)
        {
            foreach (var line in input)
            {
                int[] keys = line.Split(' ').Select(int.Parse).ToArray();

                int parentKey = keys[0];
                int childKey = keys[1];
                this.AddEdge(parentKey, childKey);
            }

            return this.GetRoot();
        }

        public IntegerTree CreateNodeByKey(int key)
        {
            if (!this.nodesByKey.ContainsKey(key))
            {
                this.nodesByKey.Add(key, new IntegerTree(key));
            }

            return this.nodesByKey[key];
        }

        public void AddEdge(int parent, int child)
        {
            var parentNode = this.CreateNodeByKey(parent);
            var childNode = this.CreateNodeByKey(child);

            parentNode.AddChild(childNode);
            childNode.AddParent(parentNode);
        }

        public IntegerTree GetRoot()
        {
            foreach (var kvp in this.nodesByKey)
            {
                if (kvp.Value.Parent is null)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
