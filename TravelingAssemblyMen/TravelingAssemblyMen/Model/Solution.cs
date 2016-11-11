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
        private List<Assembler> _team;
        private Location[] startingLocations =
            {
                new Location(-50, -50),
                new Location(50, 50),
                new Location(-50, 50),
                new Location(50, -50),
                new Location(0, -25),
                new Location(0, 25),
                new Location(25, 0),
                new Location(-25, 0)
            };

        public Int32 NumberOfAssembler
        {
            get
            {
                return _team.Count;
            }
        }

        public Solution(List<Location> customerList, int assemblerCount)
        {
            _taskDistribution = new Dictionary<Location, Int32>();
            _customerList = customerList.ToList<Location>();
            _unassignedCustomers = customerList.ToList<Location>();
            _team = new List<Assembler>();

            for (int index = 0; index < assemblerCount; index++)
            {
                if (index < startingLocations.Count())
                {
                    _team.Add(new Assembler(startingLocations[index]));
                    continue;
                }

                _team.Add(new Assembler());
            }
        }

        public void SolveRandomly()
        {
            ResetSolution();

            foreach (Location customer in _customerList)
            {
                Int32 assembler = MyRNG.Next(_team.Count);

                _taskDistribution.Add(customer, assembler);
                _team[assembler].AcceptTask(customer, CustomerInsertStyle.Random);
                _unassignedCustomers.Remove(customer);
            }
        }

        public void SolveGreedy()
        {
            ResetSolution();
            
            Assembler currentAssembler;

            //asign first customer to each assembler corresponding to their starting positions
            for (int assemblerIndex = 0; assemblerIndex < _team.Count; assemblerIndex++)
            {
                if (_customerList.Count > assemblerIndex)
                {
                    currentAssembler = _team[assemblerIndex];
                    AssignTask(FindClosestUnassigned(currentAssembler.StartingPosition), currentAssembler);
                }
            }

            int roundRobin = 0;

            while (_unassignedCustomers.Count > 0)
            {
                currentAssembler = _team[roundRobin];

                AssignTask(FindClosestUnassigned(currentAssembler.LastCustomer, currentAssembler.StartingPosition), currentAssembler);

                roundRobin = (roundRobin + 1) % _team.Count;
            }
        }

        public Double FitnessValue(Double overtimePenaltyWeight, Double overallWorkloadWeight)
        {
            Double fitness = 0;
            Double overallWorkload = 0;

            foreach (Assembler worker in _team)
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
            foreach (Assembler worker in _team)
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
            
            foreach (Assembler worker in _team)
            {
                worker.RemoveEveryTask();
            }
        }
        private Location FindClosestUnassigned(Location customer)
        {
            Location closestNeighbor = null;
            Double closestNeighborDistance = Double.MaxValue;

            foreach (Location newNeighbor in _unassignedCustomers)
            {
                Double newNeighborDistance = customer.DistanceTo(newNeighbor);

                if (newNeighborDistance < closestNeighborDistance)
                {
                    closestNeighbor = newNeighbor;
                    closestNeighborDistance = newNeighborDistance;
                }
            }

            return closestNeighbor;
        }

        private Location FindClosestUnassigned(Location customer, Location startingPosition)
        {
            Location closestNeighbor = null;
            Double closestNeighborDistance = Double.MaxValue;

            foreach (Location newNeighbor in _unassignedCustomers)
            {
                Double newNeighborDistance = customer.DistanceTo(newNeighbor) + startingPosition.DistanceTo(newNeighbor);

                if (newNeighborDistance < closestNeighborDistance)
                {
                    closestNeighbor = newNeighbor;
                    closestNeighborDistance = newNeighborDistance;
                }
            }

            return closestNeighbor;
        }

        public void TwoOpt(int neighborhoodRange)
        {
            foreach(Assembler worker in _team)
            {
                bool madeChange;

                do
                {
                    madeChange = false;

                    for (int customerindex = -1; customerindex < worker.CustomersAssigned; customerindex++)
                    {
                        bool foundCandidate = false;
                        Location candidate = null;
                        Double fitnessDelta = 0;

                        for (int neighborOffset = 1; neighborOffset <= neighborhoodRange; neighborOffset++)
                        {
                            Location piOfI = worker.CustomerAtPosition(customerindex);
                            Location piOfIPlusOne = worker.CustomerAtPosition(customerindex + 1);
                            Location piOfJ = worker.FindNeighbor(piOfI, neighborOffset);

                            if (piOfJ.Equals(Location.HQ))
                            {
                                break;
                            }

                            Location piOfJPlusOne = worker.CustomerAfter(piOfJ);

                            Double newFitnessDelta = Assembler.FitnessDelta(piOfI, piOfIPlusOne, piOfJ, piOfJPlusOne);

                            if (newFitnessDelta < fitnessDelta)
                            {
                                fitnessDelta = newFitnessDelta;
                                candidate = piOfJ;
                                foundCandidate = true;
                            }
                        }

                        if (foundCandidate)
                        {
                            worker.InvertOrder(worker.CustomerAtPosition(customerindex + 1), candidate);
                            madeChange = true;
                        }
                    }
                }
                while (madeChange);
            }
        }

        /// <summary>
        /// Returns the nth next customer in the assigned customers towards the parameter customer. 
        /// Beware that the offset 0 will result in the customer itself if it is in the list of customers;
        /// </summary>
        /// <param name="customer">The customers who's next neighbors are looked up.</param>
        /// <param name="offset">The offset how close the neighbor should be in the ranking.</param>
        /// <returns>The neighbor with an offset in the ranking.</returns>
        private Location FindGlobalNeighbor(Location customer, Int32 offset)
        {
            List<Location> ranking = _customerList.OrderBy(l=>l.DistanceTo(customer)).ToList();

            if (offset >= ranking.Count)
            {
                ranking.Last();
            }

            return ranking[offset];
        }

        private void AssignTask(Location customer, Assembler worker)
        {
            worker.AcceptTask(customer);
            _unassignedCustomers.Remove(customer);
            _taskDistribution.Add(customer, _team.IndexOf(worker));
        }
    }
}
