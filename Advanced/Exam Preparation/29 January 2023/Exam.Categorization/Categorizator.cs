using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Categorization
{
    public class Categorizator : ICategorizator
    {

        private Dictionary<string, Category> categories;

        public Categorizator()
        {
            this.categories = new Dictionary<string, Category>();
        }

        public void AddCategory(Category category)
        {
            if (categories.ContainsKey(category.Id))
            {
                throw new ArgumentException();
            }

            this.categories.Add(category.Id, category);
        }

        public void AssignParent(string childCategoryId, string parentCategoryId)
        {
            if (!categories.ContainsKey(childCategoryId) || !categories.ContainsKey(parentCategoryId))
            {
                throw new ArgumentException();
            }

            var parent = categories[parentCategoryId];
            var child = categories[childCategoryId];

            if (parent.Children.Contains(child))
            {
                throw new ArgumentException();
            }

            parent.Children.Add(child);
            child.Parent = parent;

            FixDepth(child);
        }

        public void RemoveCategory(string categoryId)
        {
            if (!this.categories.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            var category = this.categories[categoryId];

            RemoveCategoryAndChildren(category);
            
            var parent = category.Parent;

            if (parent is null)
            {
                return;
            }

            parent.Children.Remove(category);

            var childWithMaxDepth = parent.Children.OrderByDescending(c => c.Depth).FirstOrDefault();

            var otherChildren = childWithMaxDepth == null ? -1 : childWithMaxDepth.Depth;

            if (category.Depth > otherChildren)
            {
                parent.Depth = otherChildren++;
            }
        }

        public bool Contains(Category category)
            => this.categories.ContainsKey(category.Id);

        public int Size()
            => this.categories.Count;


        public IEnumerable<Category> GetChildren(string categoryId)
        {
            if (!this.categories.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            var queue = new Queue<Category>();
            var children = new List<Category>();
            queue.Enqueue(this.categories[categoryId]);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var child in current.Children)
                {
                    children.Add(child);
                    queue.Enqueue(child);
                }
            }
            return children;
        }

        public IEnumerable<Category> GetHierarchy(string categoryId)
        {
            if (!this.categories.ContainsKey(categoryId))
            {
                throw new ArgumentException();
            }

            var category = this.categories[categoryId];

            var hierarchy = new Stack<Category>();
            while (category != null)
            {
                hierarchy.Push(category);
                category = category.Parent;
            }
            return hierarchy;
        }

        public IEnumerable<Category> GetTop3CategoriesOrderedByDepthOfChildrenThenByName()
            => this.categories.Values
                .OrderByDescending(c => c.Depth)
                .ThenBy(c => c.Name)
                .Take(3);

        private void FixDepth(Category category)
        {
            while (category.Parent != null && category.Parent.Depth <= category.Depth + 1)
            {
                category.Parent.Depth = category.Depth + 1;
                category = category.Parent;
            }
        }

        private void RemoveCategoryAndChildren(Category category)
        {
            var queue = new Queue<Category>();
            queue.Enqueue(category);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                this.categories.Remove(current.Id);
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}
