using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MobileX
{
    public class VehicleRepository : IVehicleRepository
    {
        private Dictionary<string, Vehicle> vehiclesById = new Dictionary<string, Vehicle>();
        private Dictionary<string, HashSet<Vehicle>> sellerVehicles = new Dictionary<string, HashSet<Vehicle>>();

        public int Count => vehiclesById.Count;

        public bool Contains(Vehicle vehicle)
            => vehiclesById.ContainsKey(vehicle.Id);

        public void AddVehicleForSale(Vehicle vehicle, string sellerName)
        {
            if (!sellerVehicles.ContainsKey(sellerName))
            {
                sellerVehicles.Add(sellerName, new HashSet<Vehicle>());
            }

            vehicle.Seller = sellerName;
            vehiclesById.Add(vehicle.Id, vehicle);
            sellerVehicles[sellerName].Add(vehicle);
        }

        public IEnumerable<Vehicle> GetVehicles(List<string> keywords)
            => vehiclesById.Values
                .Where(v => keywords.Contains(v.Brand) ||
                            keywords.Contains(v.Model) ||
                            keywords.Contains(v.Location) ||
                            keywords.Contains(v.Color))
                .OrderByDescending(v => v.IsVIP)
                .ThenBy(v => v.Price);

        public IEnumerable<Vehicle> GetVehiclesBySeller(string sellerName)
        {
            if (!sellerVehicles.ContainsKey(sellerName))
            {
                throw new ArgumentException();
            }

            return sellerVehicles[sellerName];
        }

        public IEnumerable<Vehicle> GetVehiclesInPriceRange(double lowerBound, double upperBound)
            => vehiclesById.Values
            .Where(v => v.Price >= lowerBound && v.Price <= upperBound)
            .OrderByDescending(v => v.Horsepower);

        public Dictionary<string, List<Vehicle>> GetAllVehiclesGroupedByBrand()
        {
            if (Count == 0)
            {
                throw new ArgumentException();
            }

            var result = new Dictionary<string, SortedSet<Vehicle>>();

            foreach (var vehicle in vehiclesById.Values)
            {
                if (!result.ContainsKey(vehicle.Brand))
                {
                    result.Add(vehicle.Brand, new SortedSet<Vehicle>());
                }

                result[vehicle.Brand].Add(vehicle);
            }

            return result
                .ToDictionary(k => k.Key, v => v.Value.ToList());
        }

        public void RemoveVehicle(string vehicleId)
        {
            if (!vehiclesById.ContainsKey(vehicleId))
            {
                throw new ArgumentException();
            }

            var vehicle = vehiclesById[vehicleId];
            vehiclesById.Remove(vehicleId);
            sellerVehicles[vehicle.Seller].Remove(vehicle);
        }

        public IEnumerable<Vehicle> GetAllVehiclesOrderedByHorsepowerDescendingThenByPriceThenBySellerName()
            => vehiclesById.Values
            .OrderByDescending(v => v.Horsepower)
            .ThenBy(v => v.Price)
            .ThenBy(v => v.Seller);

        public Vehicle BuyCheapestFromSeller(string sellerName)
        {
            if (!sellerVehicles.ContainsKey(sellerName) || sellerVehicles[sellerName].Count == 0)
            {
                throw new ArgumentException();
            }

            var vehicleToBuy = sellerVehicles[sellerName].OrderBy(v => v.Price).First();
            vehiclesById.Remove(vehicleToBuy.Id);
            sellerVehicles[sellerName].Remove(vehicleToBuy);

            return vehicleToBuy;
        }
    }
}