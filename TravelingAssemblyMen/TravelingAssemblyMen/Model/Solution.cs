﻿using System;
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
        private TAP _task;

        public Int32 NumberOfAssembler
        {
            get
            {
                return _team.Count;
            }
        }

        public Solution(TAP task, Int32 numberOfAssembler)
        {
            _task = task;
            _team = new List<Assembler>();

            for (int index = 0; index < numberOfAssembler; index++)
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

            List<Location> unassignedCustomers = _task.Customers.ToList();

            while (unassignedCustomers.Count > 0)
            {
                Location nextCustomer = unassignedCustomers[MyRNG.Next(unassignedCustomers.Count)];
                Assembler worker = _team[MyRNG.Next(NumberOfAssembler)];

                AssignTask(nextCustomer, worker, unassignedCustomers);
            }
        }

        public void SolveGreedy()
        {
            ResetSolution();

            List<Location> unassignedCustomers = _task.Customers.ToList();
            Assembler currentAssembler;

            //asign first customer to each assembler corresponding to their starting positions
            for (int assemblerIndex = 0; assemblerIndex < _team.Count; assemblerIndex++)
            {
                if (_task.NumberOfCustomers > assemblerIndex)
                {
                    currentAssembler = _team[assemblerIndex];
                    Location nextCustomer = FindClosestUnassigned(currentAssembler.StartingPosition, unassignedCustomers);
                    AssignTask(nextCustomer, currentAssembler, unassignedCustomers);
                }
            }

            int roundRobin = 0;

            while (unassignedCustomers.Count > 0)
            {
                currentAssembler = _team[roundRobin];

                AssignTask(FindClosestUnassigned(currentAssembler.LastCustomer, currentAssembler.StartingPosition, unassignedCustomers), currentAssembler, unassignedCustomers);

                roundRobin = (roundRobin + 1) % _team.Count;
            }
        }

        public Double FitnessValue(Double overtimePenaltyWeight, Double overallWorkloadWeight)
        {
            Double fitness = 0;
            Double overallWorkload = 0;

            foreach (Assembler worker in _team)
            {
                Double oldWorkload = worker.Workload;

                overallWorkload += worker.Workload;
                fitness += worker.Workload + Math.Max((worker.Workload - 8) * overtimePenaltyWeight, 0);
            }

            fitness += overallWorkload * overallWorkloadWeight;

            return fitness;
        }

        internal void DrawSolution(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            foreach (Assembler worker in _team)
            {
                worker.Draw(graphics, origin, pixelsPerKilometer, ColorPicker.NextColor);
            }

            foreach (Location customer in _task.Customers)
            {
                customer.Draw(graphics, origin, pixelsPerKilometer);
            }
        }

        private void ResetSolution()
        {
            foreach (Assembler worker in _team)
            {
                worker.RemoveEveryTask();
            }
        }
        private Location FindClosestUnassigned(Location customer, List<Location> unassignedCustomers)
        {
            Location closestNeighbor = null;
            Double closestNeighborDistance = Double.MaxValue;

            foreach (Location newNeighbor in unassignedCustomers)
            {
                Double newNeighborDistance = _task.DistanceBetween(customer, newNeighbor);

                if (newNeighborDistance < closestNeighborDistance)
                {
                    closestNeighbor = newNeighbor;
                    closestNeighborDistance = newNeighborDistance;
                }
            }

            return closestNeighbor;
        }


        private Location FindClosestUnassigned(Location customer, Location startingPosition, List<Location> unassignedCustomers)
        {
            Location closestNeighbor = null;
            Double closestNeighborDistance = Double.MaxValue;

            foreach (Location newNeighbor in unassignedCustomers)
            {
                Double newNeighborDistance = _task.DistanceBetween(customer, newNeighbor) + _task.DistanceBetween(startingPosition, newNeighbor);

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

                    for (int customerindex = -1; customerindex < worker.CustomersAssigned.Count; customerindex++)
                    {
                        bool foundCandidate = false;
                        Location candidate = null;
                        Double fitnessDelta = 0;

                        Location customer = worker.CustomerAt(customerindex);
                        List<Location> neighborhood = FindSubrouteNeighbors(customer, neighborhoodRange, worker);

                        foreach (Location neighbor in neighborhood)
                        {
                            Location piOfI = customer;
                            Location piOfJ = neighbor;

                            if (worker.PositionOf(customer) > worker.PositionOf(neighbor))
                            {
                                piOfI = neighbor;
                                piOfJ = customer;
                            }

                            Location piOfIPlusOne = worker.CustomerAfter(piOfI);
                            Location piOfJPlusOne = worker.CustomerAfter(piOfJ);
                            
                            Double newFitnessDelta = FitnessDelta(piOfI, piOfIPlusOne, piOfJ, piOfJPlusOne);

                            if (newFitnessDelta < fitnessDelta)
                            {
                                fitnessDelta = newFitnessDelta;
                                candidate = neighbor;
                                foundCandidate = true;
                            }
                        }

                        if (foundCandidate)
                        {
                            Location reverseStart = worker.CustomerAfter(customer);
                            Location reverseEnd = candidate;

                            if (worker.PositionOf(customer) > worker.PositionOf(candidate))
                            {
                                reverseStart = worker.CustomerAfter(candidate);
                                reverseEnd = customer;
                            }

                            worker.InvertOrder(reverseStart, reverseEnd);
                            worker.Workload += fitnessDelta;
                            madeChange = true;
                        }
                    }
                }
                while (madeChange);
            }
        }

        public void SwapOpt(int neighborhoodRange)
        {
            if (_team.Count < 2)
            {
                return;
            }

            bool madeChange;

            do
            {
                madeChange = false;

                foreach (Assembler worker in _team)
                {
                    for (int customerindex = 0; customerindex < worker.CustomersAssigned.Count; customerindex++)
                    {
                        bool foundCandidate = false;
                        Location candidate = null;
                        Double fitnessDeltaThis = 0;
                        double fitnessDeltaOther = 0;

                        for (int neighborOffset = 1; neighborOffset <= neighborhoodRange; neighborOffset++)
                        {
                            Location currentCustomer = worker.CustomerAt(customerindex);
                            Location swapCustomer = FindGlobalNeighbor(currentCustomer, neighborOffset);

                            if (swapCustomer.Equals(Location.HQ))
                            {
                                break;
                            }

                            Assembler swapPartner = Processes(swapCustomer);

                            Double newFitnessDeltaThis = worker.FitnessDelta(currentCustomer, swapCustomer);
                            Double newFitnessDeltaOther = swapPartner.FitnessDelta(swapCustomer, currentCustomer);

                            if (newFitnessDeltaThis + newFitnessDeltaOther < fitnessDeltaThis + fitnessDeltaOther)
                            {
                                fitnessDeltaThis = newFitnessDeltaThis;
                                fitnessDeltaOther = newFitnessDeltaOther;
                                candidate = swapCustomer;
                                foundCandidate = true;
                            }
                        }

                        if (foundCandidate)
                        {
                            Assembler swapPartner = Processes(candidate);
                            Location customer = worker.CustomerAt(customerindex);

                            worker.Swap(customer, candidate);
                            worker.Workload += fitnessDeltaThis;

                            swapPartner.Swap(candidate, customer);
                            swapPartner.Workload += fitnessDeltaOther;

                            madeChange = true;
                        }
                    }
                }
            }
            while (madeChange);
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
            Assembler processesCustomer = Processes(customer);
            List<Location> ranking = _task.Customers.Where(c => Processes(c) != processesCustomer).ToList();

            ranking = ranking.OrderBy(l => _task.DistanceBetween(l, customer)).ToList();
            
            if (ranking.Count == 0)
            {
                return Location.HQ;
            }

            if (offset >= ranking.Count)
            {
                return ranking.Last();
            }

            return ranking[offset];
        }
        
        /// <summary>
        /// Returns the nth next customer in the assigned customers towards the parameter customer. 
        /// Beware that if the customer is asigned the offset 0 will result in the customer itself;
        /// </summary>
        /// <param name="customer">The customers who's next neighbors are looked up.</param>
        /// <param name="offset">The offset how close the neighbor should be in the ranking.</param>
        /// <param name="subroutes">The assemblers of which the potential neighbors should be included.</param>
        /// <returns>The neighbor with an offset in the ranking.</returns>
        private List<Location> FindSubrouteNeighbors(Location customer, Int32 maxOffset, params Assembler[] subroutes)
        {
            List<Location> ranking = new List<Location>();

            foreach (Assembler route in subroutes)
            {
                ranking.AddRange(route.CustomersAssigned);
            }

            ranking.Remove(customer);
            ranking = ranking.OrderBy(l => _task.DistanceBetween(l, customer)).ToList();

            if (maxOffset >= ranking.Count)
            {
                return ranking;
            }

            ranking.RemoveRange(maxOffset, ranking.Count - maxOffset);
            return ranking;
        }
        
        public Double FitnessDelta(Location piOfI, Location piOfIPlusOne, Location piOfJ, Location piOfJPlusOne)
        {
            Double fitnessDelta = 0;

            fitnessDelta -= _task.DistanceBetween(piOfI, piOfIPlusOne) + _task.DistanceBetween(piOfJ, piOfJPlusOne);
            fitnessDelta += _task.DistanceBetween(piOfI, piOfJ) + _task.DistanceBetween(piOfIPlusOne, piOfJPlusOne);

            return fitnessDelta;
        }

        private void AssignTask(Location customer, Assembler worker, List<Location> unassignedCustomers)
        {
            Double fitnessDelta = 0;

            if (worker.CustomersAssigned.Count > 0)
            {
                fitnessDelta -= _task.DistanceBetween(worker.CustomerBefore(worker.LastCustomer), worker.LastCustomer) + _task.DistanceBetween(worker.LastCustomer, Location.HQ);
                fitnessDelta += _task.DistanceBetween(worker.LastCustomer, customer) + _task.DistanceBetween(customer, Location.HQ);
            }
            else
            {
                fitnessDelta += _task.DistanceBetween(Location.HQ, customer) + _task.DistanceBetween(customer, Location.HQ);
            }

            worker.AcceptTask(customer, fitnessDelta);
            unassignedCustomers.Remove(customer);
        }

        private Assembler Processes(Location customer)
        {
            foreach (Assembler worker in _team)
            {
                if (worker.Processes(customer))
                {
                    return worker;
                }
            }

            return null;
        }
    }
}
