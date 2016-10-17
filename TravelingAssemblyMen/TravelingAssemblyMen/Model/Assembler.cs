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

        public Assembler()
        {
            _customersAssigned = new List<Location>();
            _workload = 0;
            _workloadIsDirty = true;
        }

        public void AcceptTask(Location newCustomer)
        {
            _InsertEfficiently(newCustomer);
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

            if (style == CustomerInsertStyle.Efficient)
            {
                _InsertEfficiently(newCustomer);
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

        /// <summary>
        /// Inserts a new customer into the most efficient position
        /// </summary>
        /// <param name="newCustomer">The new customer that needs to be accepted into the assigned customer list and put into place.</param>
        private void _InsertEfficiently(Location newCustomer)
        {
            if (_customersAssigned.Contains(newCustomer))
            {
                return;
            }

            _customersAssigned.Add(newCustomer);
        }

        public void Draw(Graphics graphics, Position origin, double pixelsPerKilometer)
        {
            Location headquarters = new Location(0, 0);
            Location currentLocation = headquarters;

            for (int taskIndex = 0; taskIndex < _customersAssigned.Count; taskIndex++)
            {
                Location nextCustomer = _customersAssigned[taskIndex];

                currentLocation.DrawLineTo(nextCustomer, graphics, origin, pixelsPerKilometer);

                currentLocation = nextCustomer;
            }

            currentLocation.DrawLineTo(headquarters, graphics, origin, pixelsPerKilometer);
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
    }
}
