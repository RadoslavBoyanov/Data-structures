namespace _01.Inventory
{
    using _01.Inventory.Interfaces;
    using _01.Inventory.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Inventory : IHolder
    {
        private List<IWeapon> weapons = new List<IWeapon>();

        public int Capacity => this.weapons.Count;

        public void Add(IWeapon weapon)
        {
            this.weapons.Add(weapon);    
        }

        public IWeapon GetById(int id)
            => this.weapons.FirstOrDefault(w=>w.Id == id);

        public bool Contains(IWeapon weapon) => this.weapons.Contains(weapon);

        public int Refill(IWeapon weapon, int ammunition)
        {
            this.IsWeaponExist(weapon);

            var weaponToRefill = this.weapons.FirstOrDefault(w=> w.Id == weapon.Id);

            weaponToRefill.Ammunition += ammunition;

            if (weaponToRefill.Ammunition > weaponToRefill.MaxCapacity)
            {
                weaponToRefill.Ammunition = weaponToRefill.MaxCapacity;
            }

            return weaponToRefill.MaxCapacity;
        }
        public bool Fire(IWeapon weapon, int ammunition)
        {
            this.IsWeaponExist(weapon);

            var weaponToFire = this.weapons.FirstOrDefault(w => w.Id == weapon.Id);

            if (weaponToFire.Ammunition < ammunition)
            {
                return false;
            }
            weaponToFire.Ammunition -= ammunition;
            return true;
        }

        public IWeapon RemoveById(int id)
        {
            var weapon = this.weapons.FirstOrDefault(w=> w.Id==id);
            this.IsWeaponExist(weapon);
            this.weapons.Remove(weapon);
            return weapon;
        }

        public void Clear() => this.weapons.Clear();

        public List<IWeapon> RetrieveAll() => new List<IWeapon>(this.weapons);

        public void Swap(IWeapon firstWeapon, IWeapon secondWeapon)
        {
            int indexOne = this.weapons.IndexOf(firstWeapon);
            this.ValidateIndex(indexOne);
            int indexTwo = this.weapons.IndexOf(secondWeapon);
            this.ValidateIndex(indexTwo);

            if (firstWeapon.Category == secondWeapon.Category)
            {
                this.weapons[indexOne] = secondWeapon;
                this.weapons[indexTwo] = firstWeapon;
            }
        }

        public List<IWeapon> RetriveInRange(Category lower, Category upper)
            => this.weapons.Where(w=>w.Category >= lower && w.Category <= upper).ToList();

        public void EmptyArsenal(Category category)
        {
            foreach (var weapon in this.weapons)
            {
                if (weapon.Category == category)
                {
                    weapon.Ammunition = 0;
                }
            }
        }
        public int RemoveHeavy()
            => this.weapons.RemoveAll(w => w.Category == Category.Heavy);

        public IEnumerator GetEnumerator()
            => this.weapons.GetEnumerator();

        private void IsWeaponExist(IWeapon weapon)
        {
            if (!this.weapons.Contains(weapon))
                throw new InvalidOperationException("Weapon does not exist in inventory!");
            
        }

        private void ValidateIndex(int index)
        {
            if (index == -1)
                throw new InvalidOperationException("Weapon does not exist in inventory!");
            
        }
    }
}
