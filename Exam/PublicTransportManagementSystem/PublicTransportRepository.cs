using System;
using System.Collections.Generic;
using System.Linq;

namespace PublicTransportManagementSystem
{
    public class PublicTransportRepository : IPublicTransportRepository
    {
        private Dictionary<string, Bus> bussesById = new Dictionary<string, Bus>();

        private Dictionary<string, Passenger> passengersById = new Dictionary<string, Passenger>();

        public void RegisterPassenger(Passenger passenger)
        {
            if (!this.passengersById.ContainsKey(passenger.Id))
            {
                this.passengersById.Add(passenger.Id, passenger);
            }
        }

        public void AddBus(Bus bus)
        {
            if (!this.bussesById.ContainsKey(bus.Id))
            {
                this.bussesById.Add(bus.Id, bus);
            }
        }

        public bool Contains(Passenger passenger) => this.passengersById.ContainsKey(passenger.Id);

        public bool Contains(Bus bus) => this.bussesById.ContainsKey(bus.Id);

        public IEnumerable<Bus> GetBuses() => this.bussesById.Values;

        public void BoardBus(Passenger passenger, Bus bus)
        {
            if (!this.bussesById.ContainsKey(bus.Id) || !this.passengersById.ContainsKey(passenger.Id))
            {
                throw new ArgumentException();
            }

            if (!this.bussesById[bus.Id].Passengers.Contains(passenger))
            {
                this.bussesById[bus.Id].Passengers.Add(passenger);
            }
            
        }

        public void LeaveBus(Passenger passenger, Bus bus)
        {
            if (!this.bussesById.ContainsKey(bus.Id) || !this.passengersById.ContainsKey(passenger.Id) || !this.bussesById[bus.Id].Passengers.Contains(passenger))
            {
                throw new ArgumentException();
            }

            this.bussesById[bus.Id].Passengers.Remove(passenger);
        }

        public IEnumerable<Passenger> GetPassengersOnBus(Bus bus)
            => this.bussesById[bus.Id].Passengers;

        public IEnumerable<Bus> GetBusesOrderedByOccupancy()
            => this.bussesById.Values.OrderBy(b => b.Passengers.Count);

        public IEnumerable<Bus> GetBusesWithCapacity(int capacity)
            => this.bussesById.Values.Where(b => b.Capacity >= capacity);
    }
}