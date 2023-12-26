using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.PackageManagerLite
{
    public class PackageManager : IPackageManager
    {
        private Dictionary<string, Package> packagesById;
        private Dictionary<string, Dictionary<string, Package>> packagesByNameAndVersion;

        public PackageManager()
        {
            this.packagesById = new Dictionary<string, Package>();
            this.packagesByNameAndVersion = new Dictionary<string, Dictionary<string, Package>>();
        }

        public void RegisterPackage(Package package)
        {
            if (this.packagesByNameAndVersion.ContainsKey(package.Name) && this.packagesByNameAndVersion[package.Name].ContainsKey(package.Version))
            {
                throw new ArgumentException();
            }
            
            this.packagesById.Add(package.Id, package);

            if (!packagesByNameAndVersion.ContainsKey(package.Name))
            {
                this.packagesByNameAndVersion.Add(package.Name, new Dictionary<string, Package>());
            }

            this.packagesByNameAndVersion[package.Name].Add(package.Version, package);
        }

        public void RemovePackage(string packageId)
        {
            if (!this.packagesById.ContainsKey(packageId))
            {
                throw new ArgumentException();
            }

            var package = this.packagesById[packageId];
            this.packagesByNameAndVersion[package.Name].Remove(package.Version);

            if (this.packagesByNameAndVersion[package.Name].Count == 0)
            {
                this.packagesByNameAndVersion.Remove(package.Name);
            }

            this.packagesById.Remove(packageId);

            foreach (var packageDependency in package.Dependants)
            {
                packageDependency.Dependencies.Remove(package);
            }

            foreach (var packageDependency in package.Dependencies)
            {
                packageDependency.Dependants.Remove(package);
            }

        }

        public void AddDependency(string packageId, string dependencyId)
        {
            if (!this.packagesById.ContainsKey(packageId) || !this.packagesById.ContainsKey(dependencyId))
            {
                throw new ArgumentException();
            }

            var package = this.packagesById[packageId];
            var dependecyPackage = this.packagesById[dependencyId];

            package.Dependencies.Add(dependecyPackage);
            dependecyPackage.Dependants.Add(package);
        }

        public bool Contains(Package package) => this.packagesById.ContainsKey(package.Id);

        public int Count() => this.packagesById.Count;

        public IEnumerable<Package> GetDependants(Package package)
        {
            if (!this.packagesById.ContainsKey(package.Id))
            {
                throw new ArgumentException();
            }

            return package.Dependants;
        }

        public IEnumerable<Package> GetIndependentPackages()
            => this.packagesById.Values
                .Where(p => p.Dependencies.Count == 0)
                .OrderByDescending(p => p.ReleaseDate)
                .ThenBy(p => p.Version);

        public IEnumerable<Package> GetOrderedPackagesByReleaseDateThenByVersion()
        {
            var result = new List<Package>();
            foreach (var packagesByVersion in packagesByNameAndVersion.Values)
            {
                var lastPackage = packagesByVersion.Values
                    .OrderByDescending(p => p.ReleaseDate)
                    .First();

                result.Add(lastPackage);
            }

            return result.OrderByDescending(p => p.ReleaseDate)
                .ThenBy(p => p.Version);
        }
    }
}
