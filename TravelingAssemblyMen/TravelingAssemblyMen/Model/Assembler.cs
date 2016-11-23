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
        #region Fields
        private List<Location> _customersAssigned;
        private Double _workload;
        private Double _travelDistance;
        private Location _startingPosition; 
        #endregion

        #region Properties
        public Double DistanceTraveled
        {
            get
            {
                return _travelDistance;
            }

            set
            {
                _travelDistance = value;
            }
        }

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
                    return Location.HQ;
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
        #endregion

        #region Constructors
        public Assembler()
        {
            _customersAssigned = new List<Location>();
            _workload = 0;
        }

        public Assembler(Location startingPosition) : this()
        {
            _startingPosition = startingPosition;
        } 
        #endregion

        #region WorkManipulation
        public void AcceptTask(Location newCustomer, Double distanceDelta)
        {
            _customersAssigned.Add(newCustomer);

            _workload += 0.5;
            _travelDistance += distanceDelta;
        }
        
        public void RemoveTask(Location lostCustomer, Double distanceDelta)
        {
            _customersAssigned.Remove(lostCustomer);

            _workload -= 0.5;
            _travelDistance += distanceDelta;
        }

        public void RemoveEveryTask()
        {
            _customersAssigned.Clear();
            _workload = 0;
            _travelDistance = 0;
        }

        public void InvertOrder(Location firstReversed, Location lastReversed, Double distanceDelta)
        {

            Int32 startIndex = PositionOf(firstReversed);
            Int32 endIndex = PositionOf(lastReversed);

            _customersAssigned.Reverse(startIndex, endIndex - startIndex + 1);
            _travelDistance += distanceDelta;
        }

        public void Swap(Location customer, Location swapper, Double distanceDelta)
        {
            Int32 customerIndex = _customersAssigned.IndexOf(customer);

            _customersAssigned.Remove(customer);
            _customersAssigned.Insert(customerIndex, swapper);
            
            _travelDistance += distanceDelta;

            return;
        } 
        #endregion

        #region Visualisation
        public void Draw(Graphics graphics, Position origin, double pixelsPerKilometer, Color lineColor)
        {
            Location headquarters = Location.HQ;
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
        #endregion

        #region Utility
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

        public bool Processes(Location customer)
        {
            return _customersAssigned.Contains(customer);
        } 

        /// <summary>
        /// Calculates the resulting distance delta if a swap of thisCustomer with otherCustomer would occur.
        /// </summary>
        /// <param name="thisCustomer">The customer which will be removed.</param>
        /// <param name="otherCustomer">The customer which will be added.</param>
        /// <returns>The delta of the fitness value as a raw distance value.</returns>
        public Double DistanceDelta(Location thisCustomer, Location otherCustomer)
        {
            Double distanceDelta = 0;

            Location thisPredecessor = this.CustomerBefore(thisCustomer);
            Location thisSuccessor = this.CustomerAfter(thisCustomer);

            distanceDelta -= 
                thisPredecessor.DistanceTo(thisCustomer) + 
                thisCustomer.DistanceTo(thisSuccessor);

            distanceDelta +=
                thisPredecessor.DistanceTo(otherCustomer) +
                otherCustomer.DistanceTo(thisSuccessor);

            return distanceDelta;
        }
        #endregion
    }
}
