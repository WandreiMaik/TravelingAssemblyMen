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
                if (_workloadIsDirty)
                {
                    _CalculateWorkload();
                }

                return _workload;
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

        public Int32 CustomersAssigned
        {
            get
            {
                return _customersAssigned.Count;
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

        public void AcceptTask(Location newCustomer)
        {
            _InsertAppending(newCustomer);
            _workloadIsDirty = true;
        }

        public void AcceptTask(Location newCustomer, CustomerInsertStyle style)
        {
            if (style == CustomerInsertStyle.Random)
            {
                _InsertRandomly(newCustomer);
                _workloadIsDirty = true;
                return;
            }

            if (style == CustomerInsertStyle.Append)
            {
                _InsertAppending(newCustomer);
                _workloadIsDirty = true;
                return;
            }

            AcceptTask(newCustomer);
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

            _customersAssigned.Reverse(startIndex, endIndex - startIndex + 1);
            _workloadIsDirty = true;
        }

        public Location CustomerAtPosition(int index)
        {
            if (index == -1 || index == _customersAssigned.Count)
            {
                return Location.HQ;
            }

            return _customersAssigned[index];
        }

        /// <summary>
        /// Returns the nth next customer in the assigned customers towards the parameter customer. 
        /// Beware that if the customer is asigned the offset 0 will result in the customer itself;
        /// </summary>
        /// <param name="customer">The customers who's next neighbors are looked up.</param>
        /// <param name="offset">The offset how close the neighbor should be in the ranking.</param>
        /// <returns>The neighbor with an offset in the ranking.</returns>
        public Location FindNeighbor(Location customer, Int32 offset)
        {
            List<Location> ranking = _customersAssigned.OrderBy(l => l.DistanceTo(customer)).ToList();

            if (offset >= ranking.Count)
            {
                return ranking.Last();
            }

            return ranking[offset];
        }

        public static Double FitnessDelta(Location piOfI, Location piOfIPlusOne, Location piOfJ, Location piOfJPlusOne)
        {
            Double fitnessDelta = 0;

            fitnessDelta -= piOfI.DistanceTo(piOfIPlusOne) + piOfJ.DistanceTo(piOfJPlusOne);
            fitnessDelta += piOfI.DistanceTo(piOfJ) + piOfIPlusOne.DistanceTo(piOfJPlusOne);

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

                currentLocation.DrawLineTo(nextCustomer, graphics, origin, pixelsPerKilometer, lineColor);

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

        /// <summary>
        /// Calculates the workload for this assembler assuming every task takes about half an hour and he is able to drive towards every customer with an average velocity of 50 km/h.
        /// </summary>
        /// <returns>The necessary workload in hours.</returns>
        private void _CalculateWorkload()
        {
            _workload = 0;
            Location headquarters = new Location(0, 0);
            Location currentLocation = headquarters;

            for (int taskIndex = 0; taskIndex < _customersAssigned.Count; taskIndex++)
            {
                Location nextCustomer = _customersAssigned[taskIndex];
                Double travelDistance = nextCustomer.DistanceTo(currentLocation);
                // assuming the assembler can travel towards the next customer with an average velocity of 50 km/h
                Double travelTime = travelDistance / 50;
                
                // assuming every task takes around half an hour
                _workload += 0.5;
                _workload += travelTime;
                currentLocation = nextCustomer;
            }

            Double distanceBackToHQ = headquarters.DistanceTo(currentLocation);
            Double timeToHome = distanceBackToHQ / 50;

            _workload += timeToHome;

            _workloadIsDirty = false;
        }

        public Location CustomerAfter(Location customer)
        {
            int indexOfCustomer = _customersAssigned.IndexOf(customer);

            return CustomerAtPosition(indexOfCustomer + 1);
        }
    }
}
