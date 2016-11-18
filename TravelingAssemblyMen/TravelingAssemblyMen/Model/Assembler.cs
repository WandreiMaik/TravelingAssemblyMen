using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelingAssemblyMen.Enums;

namespace TravelingAssemblyMen.Model
{
    public class Assembler
    {
        private List<Location> _customersAssigned;
        private Double _workload;
        private bool _workloadIsDirty;
        private Location _startingPosition;

        public Double Workload
        {
            get
            {
                return _workload;
            }

            set
            {
                _workload = value;
            }
        }

        public Location LastCustomer
        {
            get
            {
                if (_customersAssigned.Count != 0)
                {
                    return _customersAssigned.Last();
                }

                return null;
            }
        }

        public Location StartingPosition
        {
            get
            {
                if (_startingPosition == null)
                {
                    return new Location(0, 0);
                }

                return _startingPosition;
            }
        }

        public List<Location> CustomersAssigned
        {
            get
            {
                return _customersAssigned;
            }
        }

        public Assembler()
        {
            _customersAssigned = new List<Location>();
            _workload = 0;
            _workloadIsDirty = true;
        }

        public Assembler(Location startingPosition) : this()
        {
            _startingPosition = startingPosition;
        }

        public void AcceptTask(Location newCustomer, Double fitnessDelta)
        {
            _InsertAppending(newCustomer);
            _workload += fitnessDelta;
        }
        
        public void RemoveTask(Location lostCustomer)
        {
            _customersAssigned.Remove(lostCustomer);
            _workloadIsDirty = true;
        }

        public void RemoveEveryTask()
        {
            _customersAssigned.Clear();
            _workloadIsDirty = true;
        }
        
        public void InvertOrder(Location firstReversed, Location lastReversed)
        {

            Int32 startIndex = _customersAssigned.IndexOf(firstReversed);
            Int32 endIndex = _customersAssigned.IndexOf(lastReversed);

            if (startIndex > endIndex)
            {
                Int32 helper = startIndex;
                startIndex = endIndex;
                endIndex = helper;
            }
            
            _customersAssigned.Reverse(startIndex, endIndex - startIndex + 1);
        }

        internal void Swap(Location customer, Location swapper)
        {
            Int32 customerIndex = _customersAssigned.IndexOf(customer);

            _customersAssigned.Remove(customer);
            _customersAssigned.Insert(customerIndex, swapper);

            return;
        }
        
        public Double FitnessDelta(Location thisCustomer, Location otherCustomer)
        {
            Double fitnessDelta = 0;

            Location thisPredecessor = this.CustomerBefore(thisCustomer);
            Location thisSuccessor = this.CustomerAfter(thisCustomer);

            fitnessDelta -= 
                thisPredecessor.DistanceTo(thisCustomer) + 
                thisCustomer.DistanceTo(thisSuccessor);

            fitnessDelta +=
                thisPredecessor.DistanceTo(otherCustomer) +
                otherCustomer.DistanceTo(thisSuccessor);

            return fitnessDelta;
        }

        /// <summary>
        /// Inserts a new customer at the end
        /// </summary>
        /// <param name="newCustomer">The new customer that needs to be accepted into the assigned customer list and put into place.</param>
        private void _InsertAppending(Location newCustomer)
        {
            if (_customersAssigned.Contains(newCustomer))
            {
                return;
            }

            _customersAssigned.Add(newCustomer);
        }

        public void Draw(Graphics graphics, Position origin, double pixelsPerKilometer, Color lineColor)
        {
            Location headquarters = new Location(0, 0);
            Location currentLocation = headquarters;

            for (int taskIndex = 0; taskIndex < _customersAssigned.Count; taskIndex++)
            {
                Location nextCustomer = _customersAssigned[taskIndex];

                if (taskIndex == 0)
                {
                    currentLocation.DrawLineTo(nextCustomer, graphics, origin, pixelsPerKilometer, Color.Black);
                }
                else
                {
                    currentLocation.DrawLineTo(nextCustomer, graphics, origin, pixelsPerKilometer, lineColor);
                }

                currentLocation = nextCustomer;
            }

            currentLocation.DrawLineTo(headquarters, graphics, origin, pixelsPerKilometer, lineColor);
        }

        /// <summary>
        /// Inserts a new customer into a random position
        /// </summary>
        /// <param name="newCustomer">The new customer that needs to be accepted into the assigned customer list and put into place.</param>
        private void _InsertRandomly(Location newCustomer)
        {
            if (_customersAssigned.Contains(newCustomer))
            {
                return;
            }

            _customersAssigned.Insert(MyRNG.Next(_customersAssigned.Count + 1), newCustomer);
        }
        
        public Location CustomerAt(int index)
        {
            if (index == -1 || index == _customersAssigned.Count)
            {
                return Location.HQ;
            }

            return _customersAssigned[index];
        }

        public Location CustomerAfter(Location customer)
        {
            int indexOfCustomer = _customersAssigned.IndexOf(customer);

            return CustomerAt(indexOfCustomer + 1);
        }

        public Location CustomerBefore(Location customer)
        {
            int indexOfCustomer = _customersAssigned.IndexOf(customer);

            return CustomerAt(indexOfCustomer - 1);
        }

        public Int32 PositionOf(Location customer)
        {
            return _customersAssigned.IndexOf(customer);
        }

        internal bool Processes(Location customer)
        {
            return _customersAssigned.Contains(customer);
        }
    }
}
