using System.Collections.Generic;

namespace Exam.Categorization
{
    public class Category
    {
        public Category(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Children = new HashSet<Category>();
            Depth = 0;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Category Parent { get; set; }

        public HashSet<Category> Children { get; set; }

        public int Depth { get; set; }
    }
}
