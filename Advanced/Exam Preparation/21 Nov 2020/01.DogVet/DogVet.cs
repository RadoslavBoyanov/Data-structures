using System.Linq;

namespace _01.DogVet
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class DogVet : IDogVet
    {
        private Dictionary<string, Dog> dogsById;
        private Dictionary<string, Dictionary<string, Dog>> dogsByOwnersAndName;

        public DogVet()
        {
            this.dogsById = new Dictionary<string, Dog>();
            this.dogsByOwnersAndName = new Dictionary<string, Dictionary<string, Dog>>();
        }

        public void AddDog(Dog dog, Owner owner)
        {
            if (this.dogsById.ContainsKey(dog.Id))
            {
                throw new ArgumentException();
            }

            if (this.dogsByOwnersAndName.ContainsKey(owner.Id) && this.dogsByOwnersAndName[owner.Id].ContainsKey(dog.Name))
            {
                throw new ArgumentException();
            }

            if (!this.dogsByOwnersAndName.ContainsKey(owner.Id))
            {
                this.dogsByOwnersAndName[owner.Id] = new Dictionary<string, Dog>();
            }

            dog.Owner = owner;
            owner.Dogs.Add(dog);
            this.dogsById.Add(dog.Id, dog);
            this.dogsByOwnersAndName[owner.Id].Add(dog.Name, dog);
        }

        public bool Contains(Dog dog)
            => this.dogsById.ContainsKey(dog.Id);

        public int Size => this.dogsById.Count;

        public Dog GetDog(string name, string ownerId)
        {
            if (!this.dogsByOwnersAndName.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            if (!this.dogsByOwnersAndName[ownerId].ContainsKey(name))
            {
                throw new ArgumentException();
            }

            return this.dogsByOwnersAndName[ownerId][name];
        }

        public Dog RemoveDog(string name, string ownerId)
        {
            if (!this.dogsByOwnersAndName.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            if (!this.dogsByOwnersAndName[ownerId].ContainsKey(name))
            {
                throw new ArgumentException();
            }

            var dogForRemove = this.dogsByOwnersAndName[ownerId][name];
            this.dogsById.Remove(dogForRemove.Id);
            this.dogsByOwnersAndName[ownerId].Remove(name);
            if (dogsByOwnersAndName[ownerId].Count == 0)
            {
                this.dogsByOwnersAndName.Remove(ownerId);
            }

            return dogForRemove;
        }

        public IEnumerable<Dog> GetDogsByOwner(string ownerId)
        {
            if (!this.dogsByOwnersAndName.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            return this.dogsByOwnersAndName[ownerId].Values.ToList();
        }

        public IEnumerable<Dog> GetDogsByBreed(Breed breed)
        {
            var dogsByBreed = this.dogsById.Values
                .Where(d => d.Breed == breed);

            if (dogsByBreed.Count() == 0)
            {
                throw new ArgumentException();
            }

            return dogsByBreed;
        }

        public void Vaccinate(string name, string ownerId)
        {
            if (!this.dogsByOwnersAndName.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            if (!this.dogsByOwnersAndName[ownerId].ContainsKey(name))
            {
                throw new ArgumentException();
            }

            var dog = this.dogsByOwnersAndName[ownerId][name];

            dog.Vaccines++;
        }

        public void Rename(string oldName, string newName, string ownerId)
        {
            if (!this.dogsByOwnersAndName.ContainsKey(ownerId))
            {
                throw new ArgumentException();
            }

            if (!this.dogsByOwnersAndName[ownerId].ContainsKey(oldName))
            {
                throw new ArgumentException();
            }

            if (this.dogsByOwnersAndName[ownerId].ContainsKey(newName))
            {
                throw new ArgumentException();
            }

            var dog = this.dogsByOwnersAndName[ownerId][oldName];
            dog.Name = newName;
            this.dogsByOwnersAndName[ownerId].Remove(oldName);
            this.dogsByOwnersAndName[ownerId][newName] = dog;

        }

        public IEnumerable<Dog> GetAllDogsByAge(int age)
        {
            var allDogs = this.dogsById.Values
                .Where(d => d.Age == age);

            if (allDogs.Count() == 0)
            {
                throw new ArgumentException();
            }

            return allDogs;
        }

        public IEnumerable<Dog> GetDogsInAgeRange(int lo, int hi)
            => this.dogsById.Values
                .Where(d => d.Age >= lo && d.Age <= hi);

        public IEnumerable<Dog> GetAllOrderedByAgeThenByNameThenByOwnerNameAscending()
            => this.dogsById.Values
                .OrderBy(d => d.Age)
                .ThenBy(d => d.Name)
                .ThenBy(d => d.Owner.Name);
    }
}