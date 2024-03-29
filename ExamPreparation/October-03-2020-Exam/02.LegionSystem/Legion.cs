﻿namespace _02.LegionSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using _02.LegionSystem.Interfaces;

    public class Legion : IArmy
    {
        private SortedSet<IEnemy> enemies = new SortedSet<IEnemy>();

        public int Size => this.enemies.Count;

        public void Create(IEnemy enemy) => this.enemies.Add(enemy);

        public IEnemy GetByAttackSpeed(int speed)
            => this.enemies.FirstOrDefault(e => e.AttackSpeed == speed);

        public bool Contains(IEnemy enemy)
            => this.enemies.Contains(enemy);

        public IEnemy GetFastest()
        {
            this.IfLegionIsEmtpyThrowException();

            return this.enemies.Max;
        }

        public IEnemy GetSlowest()
        {
            this.IfLegionIsEmtpyThrowException();

            return this.enemies.Min;
        }

        public void ShootFastest()
        {
            this.IfLegionIsEmtpyThrowException();;

            this.enemies.Remove(this.enemies.Max);
        }

        public void ShootSlowest()
        {
            this.IfLegionIsEmtpyThrowException();

            this.enemies.Remove(this.enemies.Min);
        }

        public IEnemy[] GetOrderedByHealth()
            => this.enemies.OrderByDescending(e=>e.Health).ToArray();

        public List<IEnemy> GetFaster(int speed)
        => this.enemies.Where(e => e.AttackSpeed > speed).ToList();

        public List<IEnemy> GetSlower(int speed)
            => this.enemies.Where(e => e.AttackSpeed < speed).ToList();

        private void IfLegionIsEmtpyThrowException()
        {
            if (this.enemies.Count == 0)
                throw new InvalidOperationException("Legion has no enemies!");
        }
    }
}
