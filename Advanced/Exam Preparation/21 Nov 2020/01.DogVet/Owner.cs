using System.Collections.Generic;

namespace _01.DogVet
{
    public class Owner
    {
        public Owner(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Dogs = new HashSet<Dog>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public HashSet<Dog> Dogs { get; set; }
    }
}