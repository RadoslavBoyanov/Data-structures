using System.Collections;
using System.Collections.Generic;

namespace PublicTransportManagementSystem
{
    public class Bus
    {
        public string Id { get; set; }
    
        public string Number { get; set; }
    
        public int Capacity { get; set; }

        public ICollection<Passenger> Passengers { get; set; }

        public Bus()
        {
            this.Passengers= new HashSet<Passenger>();
        }


        public override string ToString()
        {
            return this.Id;
        }
    }
}