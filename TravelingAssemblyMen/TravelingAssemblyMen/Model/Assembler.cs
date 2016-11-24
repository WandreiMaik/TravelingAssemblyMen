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
        private TAP _task;
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
        public Assembler(TAP task)
        {
            _customersAssigned = new List<Location>();
            _workload = 0;
            _task = task;
        }

        public Assembler(Location startingPosition, TAP task) : this(task)
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
            Double realDistanceTraveled = CalculateDistanceTraveled();

            if (_travelDistance != realDistanceTraveled)
            {
                double diff = realDistanceTraveled - _travelDistance;
            }
        }

        public void Swap(Location removeCustomer, Location insertCustomer, Int32 insertIndex, Double distanceDelta)
        {

            Location customerBeforeRemoved = CustomerBefore(removeCustomer);
            Location customerAfterRemoved = CustomerAfter(removeCustomer);
            
            _customersAssigned.Remove(removeCustomer);
            _customersAssigned.Insert(insertIndex, insertCustomer);

            _travelDistance += distanceDelta;

            if (_travelDistance != CalculateDistanceTraveled())
            {
                double diff = 0;
            }
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
        public Location CustomerBefore(Location customer)
        {
            int indexOfCustomer = _customersAssigned.IndexOf(customer);

            if (Location.HQ.Equals(customer))
            {
                return LastCustomer;
            }

            if (!_customersAssigned.Contains(customer))
            {
                return null;
            }

            return CustomerAt(indexOfCustomer - 1);
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

            if (Location.HQ.Equals(customer))
            {
                return CustomerAt(0);
            }

            if (!_customersAssigned.Contains(customer))
            {
                return null;
            }

            return CustomerAt(indexOfCustomer + 1);
        }

        public Int32 PositionOf(Location customer)
        {
            return _customersAssigned.IndexOf(customer);
        }

        public bool Processes(Location customer)
        {
            return _customersAssigned.Contains(customer) || customer.Equals(Location.HQ);
        } 

        private Double CalculateDistanceTraveled()
        {
            Double distanceTraveled = _task.DistanceBetween(Location.HQ, _customersAssigned[0]);

            foreach(Location customer in _customersAssigned)
            {
                distanceTraveled += _task.DistanceBetween(customer, CustomerAfter(customer));
            }

            return distanceTraveled;
        }
        #endregion
    }
}
