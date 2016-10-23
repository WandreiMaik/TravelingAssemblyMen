using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelingAssemblyMen.Enums;

namespace TravelingAssemblyMen.Model
{
    public class Solution
    {
        /// <summary>
        /// A dictionary containing information about the distribution of all tasks (customers) between every assembler.
        /// </summary>
        private Dictionary<Location, Int32> _taskDistribution;
        private List<Location> _customerList;
        private List<Location> _unassignedCustomers;
        private List<Assembler> _assemblerList;
        private Location[] startingLocations =
            {
                new Location(-50, -50),
                new Location(50, 50),
                new Location(-50, 50),
                new Location(50, -50)
            };

        public Int32 NumberOfAssembler
        {
            get
            {
                return _assemblerList.Count;
            }
        }

        public Solution(List<Location> customerList, int assemblerCount)
        {
            _taskDistribution = new Dictionary<Location, Int32>();
            _customerList = customerList.ToList<Location>();
            _unassignedCustomers = customerList.ToList<Location>();
            _assemblerList = new List<Assembler>();

            for (int index = 0; index < assemblerCount; index++)
            {
                if (index < startingLocations.Count())
                {
                    _assemblerList.Add(new Assembler(startingLocations[index]));
                    continue;
                }

                _assemblerList.Add(new Assembler());
            }
        }

        public void SolveRandomly()
        {
            ResetSolution();

            foreach (Location customer in _customerList)
            {
                Int32 assembler = MyRNG.Next(_assemblerList.Count);

                _taskDistribution.Add(customer, assembler);
                _assemblerList[assembler].AcceptTask(customer, CustomerInsertStyle.Random);
                _unassignedCustomers.Remove(customer);
            }
        }

        public void SolveGreedy()
        {
            ResetSolution();
            
            Assembler currentAssembler;

            //asign first customer to each assembler corresponding to their starting positions
            for (int assemblerIndex = 0; assemblerIndex < _assemblerList.Count; assemblerIndex++)
            {
                if (_customerList.Count > assemblerIndex)
                {
                    currentAssembler = _assemblerList[assemblerIndex];
                    AssignTask(FindClosestUnassigned(currentAssembler.StartingPosition), currentAssembler);
                }
            }

            int roundRobin = 0;

            while (_unassignedCustomers.Count > 0)
            {
                currentAssembler = _assemblerList[roundRobin];

                AssignTask(FindClosestUnassigned(currentAssembler.LastCustomer), currentAssembler);

                roundRobin = (roundRobin + 1) % _assemblerList.Count;
            }
        }

        public Double FitnessValue(Double overtimePenaltyWeight, Double overallWorkloadWeight)
        {
            Double fitness = 0;
            Double overallWorkload = 0;

            foreach (Assembler worker in _assemblerList)
            {
                overallWorkload += worker.Workload;
                fitness += worker.Workload + Math.Max((worker.Workload - 8) * overtimePenaltyWeight, 0);
            }

            fitness += overallWorkload * overallWorkloadWeight;

            // kill every solution with customers that nobody worked at
            if (_unassignedCustomers.Count > 0)
            {
                return Double.MaxValue;
            }

            return fitness;
        }

        internal void DrawSolution(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            foreach (Assembler worker in _assemblerList)
            {
                worker.Draw(graphics, origin, pixelsPerKilometer, ColorPicker.NextColor);
            }

            foreach (Location customer in _customerList)
            {
                customer.Draw(graphics, origin, pixelsPerKilometer);
            }
        }

        private void ResetSolution()
        {
            _taskDistribution.Clear();
            _unassignedCustomers = _customerList.ToList<Location>();
            
            foreach (Assembler worker in _assemblerList)
            {
                worker.RemoveEveryTask();
            }
        }

        private Location FindClosestUnassigned(Location customer)
        {
            Location closestNeighbor = null;

            foreach (Location newNeighbor in _unassignedCustomers)
            {
                if (newNeighbor.DistanceTo(customer) < customer.DistanceTo(closestNeighbor))
                {
                    closestNeighbor = newNeighbor;
                }
            }

            return closestNeighbor;
        }

        private void AssignTask(Location customer, Assembler worker)
        {
            worker.AcceptTask(customer);
            _unassignedCustomers.Remove(customer);
            _taskDistribution.Add(customer, _assemblerList.IndexOf(worker));
        }
    }
}
