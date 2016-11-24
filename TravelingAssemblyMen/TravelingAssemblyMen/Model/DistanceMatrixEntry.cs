using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingAssemblyMen.Model
{
    public class DistanceMatrixEntry
    {
        private Location _customer;
        private Double _distance;
           
        public Location Customer
        {
            get
            {
                return _customer;
            }
        }

        public Double Distance
        {
            get
            {
                return _distance;
            }
        }

        public DistanceMatrixEntry(Location distanceToCustomer, Location EntryOfCustomer)
        {
            _customer = distanceToCustomer;
            _distance = EntryOfCustomer.DistanceTo(distanceToCustomer);
        }

        public DistanceMatrixEntry(Location distanceToCustomer, Double distance)
        {
            _customer = distanceToCustomer;
            _distance = distance;
        }

        public override String ToString()
        {
            return Customer.ToString() + " Distance: " + Distance.ToString();
        }
    }
}
