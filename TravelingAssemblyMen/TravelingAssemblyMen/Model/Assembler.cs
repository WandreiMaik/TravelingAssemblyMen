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
                return _customersAssigned.Count * 0.5d;
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
            
            _travelDistance += distanceDelta;
        }
        
        public void RemoveTask(Location lostCustomer, Double distanceDelta)
        {
            _customersAssigned.Remove(lostCustomer);
            
            _travelDistance += distanceDelta;
        }

        public void RemoveTasks(List<Location> lostCustomers, Double distanceDelta)
        {
            foreach (Location customer in lostCustomers)
            {
                _customersAssigned.Remove(customer);
            }

            _travelDistance += distanceDelta;
        }

        public void RemoveEveryTask()
        {
            _customersAssigned.Clear();
            _travelDistance = 0;
        }
        
        public void InsertTask(Location insertedCustomer, int index, double distanceDelta)
        {
            _customersAssigned.Insert(index, insertedCustomer);

            _travelDistance += distanceDelta;
        }

        public void InsertTasks(List<Location> insertedCustomers, int insertIndex, double distanceDelta)
        {
            for (int insertedCustomerIndex = 0; insertedCustomerIndex < insertedCustomers.Count; insertedCustomerIndex++)
            {
                _customersAssigned.Insert(insertIndex + insertedCustomerIndex, insertedCustomers[insertedCustomerIndex]);
            }

            _travelDistance += distanceDelta;
        }

        public void InvertOrder(Location firstReversed, Location lastReversed, Double distanceDelta)
        {
            Int32 startIndex = PositionOf(firstReversed);
            Int32 endIndex = PositionOf(lastReversed);

            _customersAssigned.Reverse(startIndex, endIndex - startIndex + 1);
            _travelDistance += distanceDelta;
        }

        public void Swap(Location removeCustomer, Location insertCustomer, Int32 insertIndex, Double distanceDelta)
        {

            Location customerBeforeRemoved = CustomerBefore(removeCustomer);
            Location customerAfterRemoved = CustomerAfter(removeCustomer);
            
            _customersAssigned.Remove(removeCustomer);
            _customersAssigned.Insert(insertIndex, insertCustomer);

            _travelDistance += distanceDelta;
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

                
                currentLocation.DrawLineTo(nextCustomer, graphics, origin, pixelsPerKilometer, lineColor);

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
                if (_customersAssigned.Count == 0)
                {
                    return Location.HQ;
                }

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
                if (_customersAssigned.Count == 0)
                {
                    return Location.HQ;
                }

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
