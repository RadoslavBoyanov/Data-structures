using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.DeliveriesManager
{
    public class AirlinesManager : IAirlinesManager
    {
        private Dictionary<string, Airline> airlinesById = new Dictionary<string, Airline>();

        private Dictionary<string, Flight> flightsById = new Dictionary<string, Flight>();

        public void AddAirline(Airline airline)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                this.airlinesById.Add(airline.Id, airline);
            }
        }

        public void AddFlight(Airline airline, Flight flight)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            if (!this.flightsById.ContainsKey(flight.Id))
            {
                this.flightsById.Add(flight.Id, flight);
            }

            this.airlinesById[airline.Id].Flights.Add(flight);
        }

        public bool Contains(Airline airline) => this.airlinesById.ContainsKey(airline.Id);

        public bool Contains(Flight flight) => this.flightsById.ContainsKey(flight.Id);

        public void DeleteAirline(Airline airline)
        {
            if (!this.airlinesById.ContainsKey(airline.Id))
            {
                throw new ArgumentException();
            }

            foreach (var flight in this.airlinesById[airline.Id].Flights)
            {
                this.flightsById.Remove(flight.Id);
            }

            this.airlinesById.Remove(airline.Id);
        }

        public IEnumerable<Airline> GetAirlinesOrderedByRatingThenByCountOfFlightsThenByName()
            => this.airlinesById.Values.OrderByDescending(a => a.Rating)
            .ThenByDescending(a => a.Flights.Count).ThenBy(a => a.Name);

        public IEnumerable<Airline> GetAirlinesWithFlightsFromOriginToDestination(string origin, string destination)
        {
            var airlines = this.airlinesById.Values.Where(a=> a.Flights
                            .Any(fl => fl.IsCompleted == false && fl.Origin == origin && fl.Destination == destination));

            return airlines;
        }

        public IEnumerable<Flight> GetAllFlights() => this.flightsById.Values;

        public IEnumerable<Flight> GetCompletedFlights()
            => this.flightsById.Values.Where(fl => fl.IsCompleted);

        public IEnumerable<Flight> GetFlightsOrderedByCompletionThenByNumber()
            => this.flightsById.Values.OrderBy(fl => fl.IsCompleted).ThenBy(fl => fl.Number);

        public Flight PerformFlight(Airline airline, Flight flight)
        {
            if (!this.airlinesById.ContainsKey(airline.Id) || !this.flightsById.ContainsKey(flight.Id))
            {
                throw new ArgumentException();
            }

            this.flightsById[flight.Id].IsCompleted = true;

            return this.flightsById[flight.Id];
        }
    }
}
