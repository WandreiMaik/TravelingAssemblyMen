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
        private List<Assembler> _team;
        private Location[] startingLocations =
            {
                new Location(-50, -50, "startingLocation0"),
                new Location(50, 50, "startingLocation1"),
                new Location(-50, 50, "startingLocation2"),
                new Location(50, -50, "startingLocation3"),
                new Location(0, -25, "startingLocation4"),
                new Location(0, 25, "startingLocation5"),
                new Location(25, 0, "startingLocation6"),
                new Location(-25, 0, "startingLocation7")
            };
        private TAP _task;
        private Double _overtimePenaltyWeight;
        private Double _overallDistanceWeight;

        public Int32 NumberOfAssembler
        {
            get
            {
                return _team.Count;
            }
        }

        public Double OvertimePenaltyWeigth
        {
            set
            {
                _overtimePenaltyWeight = value;
            }
        }

        public Double OverallWorkloadWeight
        {
            set
            {
                _overallDistanceWeight = value;
            }
        }

        public Solution(TAP task, Int32 numberOfAssembler, Double overtimePenaltyWeight, Double overallDistanceWeight)
        {
            _task = task;
            _team = new List<Assembler>();

            for (int index = 0; index < numberOfAssembler; index++)
            {
                if (index < startingLocations.Count())
                {
                    _team.Add(new Assembler(startingLocations[index], task));
                    continue;
                }

                _team.Add(new Assembler(task));
            }

            _overtimePenaltyWeight = overtimePenaltyWeight;
            _overallDistanceWeight = overallDistanceWeight;
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

            foreach (Location customer in _task.Customers)
            {
                List<DistanceMatrixEntry> customerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(customer)];
                List<Double> distanceDeltas = new List<double>();
                List<Int32> insertedIndices = new List<int>();

                foreach (Assembler worker in _team)
                {
                    Int32 insertIndex = -1;

                    distanceDeltas.Add(DistanceDelta(worker, customer, customerDistances, out insertIndex));

                    insertedIndices.Add(insertIndex);
                }

                List<Double> newDistances = new List<double>();

                foreach (Double distance in distanceDeltas)
                {
                    Assembler worker = _team[distanceDeltas.IndexOf(distance)];

                    newDistances.Add(worker.DistanceTraveled + distance);
                }

                Int32 correspondingIndex = newDistances.IndexOf(newDistances.Min());

                Assembler insertInto = _team[correspondingIndex];

                insertInto.InsertTask(customer, insertedIndices[correspondingIndex], distanceDeltas[correspondingIndex]);
            }
        }

        public Double FitnessValue()
        {
            Double fitness = 0;
            Double overallDistance = 0;

            foreach (Assembler worker in _team)
            {
                overallDistance += worker.DistanceTraveled;
                fitness += Math.Max((worker.Workload - 8) * _overtimePenaltyWeight, 0) + worker.Workload;
            }

            fitness += overallDistance / 50 * _overallDistanceWeight;

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
        
        private Location FindClosestUnassigned(Location customer, Location startingPosition, List<Location> unassignedCustomers, List<List<DistanceMatrixEntry>> modifiedDistances)
        {
            List<DistanceMatrixEntry> Neighbors = _task.Distances(customer).Where(dist => unassignedCustomers.Contains(dist.Customer)).ToList();

            if (Neighbors.First().Customer.Equals(Location.HQ))
            {
                return Neighbors.Skip(1).First().Customer;
            }

            return Neighbors.First().Customer;
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
                        Location bestCustomer = null;
                        Location bestNeighbor = null;
                        Double bestDistanceDelta = 0;

                        Location currentCustomer = worker.CustomerAt(customerindex);
                        List<Location> neighborhood = FindSubrouteNeighbors(currentCustomer, neighborhoodRange, worker);

                        foreach (Location currentNeighbor in neighborhood)
                        {
                            Location piOfIPlusOne = worker.CustomerAfter(currentCustomer);
                            Location piOfJ = currentNeighbor;
                            
                            if (piOfIPlusOne.Equals(piOfJ))
                            {
                                continue;
                            }

                            Location piOfJPlusOne = worker.CustomerAfter(piOfJ);
                            
                            Double newRawDistanceDelta = DistanceDelta(currentCustomer, piOfIPlusOne, piOfJ, piOfJPlusOne);

                            if (newRawDistanceDelta < bestDistanceDelta)
                            {
                                bestDistanceDelta = newRawDistanceDelta;
                                bestCustomer = currentCustomer;
                                bestNeighbor = currentNeighbor;
                                foundCandidate = true;
                            }
                        }

                        if (foundCandidate)
                        {
                            Location reverseStart = worker.CustomerAfter(bestCustomer);
                            Location reverseEnd = bestNeighbor;

                            if (worker.PositionOf(bestCustomer) > worker.PositionOf(bestNeighbor))
                            {
                                reverseStart = worker.CustomerAfter(bestNeighbor);
                                reverseEnd = bestCustomer;
                            }

                            worker.InvertOrder(reverseStart, reverseEnd, bestDistanceDelta);

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

                bool foundCandidate = false;
                Location bestCurrentCustomer = null;
                Location bestSwapCustomer = null;
                Double bestDistanceDeltaWorker = 0;
                Double bestDistanceDeltaSwapPartner = 0;
                Double bestFitnessDeltaWorker = 0;
                Double bestFitnessDeltaSwapPartner = 0;
                Int32 bestInsertIndexWorker = -1;
                Int32 bestInsertIndexSwapPartner = -1;
                
                foreach (Assembler worker in _team)
                {
                    for (int customerindex = 0; customerindex < worker.CustomersAssigned.Count; customerindex++)
                    {
                        Location currentCustomer = worker.CustomerAt(customerindex);

                        for (int neighborOffset = 0; neighborOffset < neighborhoodRange; neighborOffset++)
                        {
                            Location swapCustomer = FindGlobalNeighbor(currentCustomer, neighborOffset);

                            if (swapCustomer.Equals(Location.HQ))
                            {
                                break;
                            }

                            Assembler swapPartner = ProcessorOf(swapCustomer);

                            List<DistanceMatrixEntry> currentCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(currentCustomer)];
                            List<DistanceMatrixEntry> swapCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(swapCustomer)];

                            Int32 workerInsertIndex = -1;
                            Int32 swapPartnerInsertIndex = -1;

                            Double newRawDistanceDeltaWorker = DistanceDelta(worker, swapCustomer, swapCustomerDistances, currentCustomer, out workerInsertIndex);
                            Double newDistanceDeltaWorker = newRawDistanceDeltaWorker * _overallDistanceWeight;
                            Double newWorkloadDeltaWorker = (newDistanceDeltaWorker / 50) + (Math.Max((worker.Workload + (newDistanceDeltaWorker / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(worker.Workload - 8, 0) * _overtimePenaltyWeight);
                            Double newFitnessDeltaWorker = newDistanceDeltaWorker + newWorkloadDeltaWorker;

                            Double newRawDistanceDeltaSwapPartner = DistanceDelta(swapPartner, currentCustomer, currentCustomerDistances, swapCustomer, out swapPartnerInsertIndex);
                            Double newDistanceDeltaSwapPartner = newRawDistanceDeltaSwapPartner * _overallDistanceWeight;
                            Double newWorkloadDeltaSwapPartner = (newDistanceDeltaSwapPartner / 50) + (Math.Max((worker.Workload + (newDistanceDeltaSwapPartner / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(worker.Workload - 8, 0) * _overtimePenaltyWeight);
                            Double newFitnessDeltaSwapPartner = newDistanceDeltaSwapPartner + newWorkloadDeltaSwapPartner;

                            if (newFitnessDeltaWorker + newFitnessDeltaSwapPartner < bestFitnessDeltaWorker + bestFitnessDeltaSwapPartner)
                            {
                                bestDistanceDeltaWorker = newRawDistanceDeltaWorker;
                                bestDistanceDeltaSwapPartner = newRawDistanceDeltaSwapPartner;
                                bestFitnessDeltaWorker = newFitnessDeltaWorker;
                                bestFitnessDeltaSwapPartner = newFitnessDeltaSwapPartner;
                                bestCurrentCustomer = currentCustomer;
                                bestSwapCustomer = swapCustomer;
                                bestInsertIndexWorker = workerInsertIndex;
                                bestInsertIndexSwapPartner = swapPartnerInsertIndex;
                                foundCandidate = true;
                            }
                        }
                    }
                }

                if (foundCandidate)
                {
                    Assembler worker = ProcessorOf(bestCurrentCustomer);
                    Assembler swapPartner = ProcessorOf(bestSwapCustomer);
                    List<DistanceMatrixEntry> currentCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(bestCurrentCustomer)];
                    List<DistanceMatrixEntry> swapCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(bestSwapCustomer)];

                    worker.Swap(bestCurrentCustomer, bestSwapCustomer, bestInsertIndexWorker, bestDistanceDeltaWorker);

                    swapPartner.Swap(bestSwapCustomer, bestCurrentCustomer, bestInsertIndexSwapPartner, bestDistanceDeltaSwapPartner);

                    madeChange = true;
                }
            }
            while (madeChange);
        }

        public void Insert1Opt(int neighborhoodRange)
        {
            if (_team.Count < 2)
            {
                return;
            }

            bool madeChange;

            do
            {
                madeChange = false;

                bool foundCandidate = false;
                Location bestMovedCustomer = null;
                Assembler bestInsertPartner = null;
                Double bestDistanceDeltaInsert = 0;
                Double bestDistanceDeltaRemove = 0;
                Double bestFitnessDeltaInsert = 0;
                Double bestFitnessDeltaRemove = 0;
                Int32 bestInsertIndex = -1;

                foreach (Location movedCustomer in _task.Customers)
                {
                    Assembler worker = ProcessorOf(movedCustomer);

                    List<Assembler> swapPartners = _team.ToList();
                    swapPartners.Remove(worker);

                    foreach (Assembler swapPartner in swapPartners)
                    {
                        List<DistanceMatrixEntry> insertCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(movedCustomer)];

                        Int32 InsertIndex = -1;

                        Double newRawDistanceDeltaRemove = DistanceDelta(worker, movedCustomer);
                        Double newDistanceDeltaRemove = newRawDistanceDeltaRemove * _overallDistanceWeight;
                        Double newWorkloadDeltaRemove = (newDistanceDeltaRemove / 50) + (Math.Max((worker.Workload + (newDistanceDeltaRemove / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(worker.Workload - 8, 0) * _overtimePenaltyWeight);
                        Double newFitnessDeltaRemove = newDistanceDeltaRemove + newWorkloadDeltaRemove;

                        Double newRawDistanceDeltaInsert = DistanceDelta(swapPartner, movedCustomer, insertCustomerDistances, out InsertIndex);
                        Double newDistanceDeltaInsert = newRawDistanceDeltaInsert * _overallDistanceWeight;
                        Double newWorkloadDeltaInsert = (newDistanceDeltaInsert / 50) + (Math.Max((swapPartner.Workload + (newDistanceDeltaInsert / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(swapPartner.Workload - 8, 0) * _overtimePenaltyWeight);
                        Double newFitnessDeltaInsert = newDistanceDeltaInsert + newWorkloadDeltaInsert;

                        if (newFitnessDeltaRemove + newFitnessDeltaInsert < bestFitnessDeltaInsert + bestFitnessDeltaRemove)
                        {
                            bestDistanceDeltaRemove = newRawDistanceDeltaRemove;
                            bestDistanceDeltaInsert = newRawDistanceDeltaInsert;
                            bestFitnessDeltaRemove = newFitnessDeltaRemove;
                            bestFitnessDeltaInsert = newFitnessDeltaInsert;
                            bestMovedCustomer = movedCustomer;
                            bestInsertPartner = swapPartner;
                            bestInsertIndex = InsertIndex;
                            foundCandidate = true;
                        }
                    }
                }

                if (foundCandidate)
                {
                    Assembler worker = ProcessorOf(bestMovedCustomer);
                    List<DistanceMatrixEntry> insertCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(bestMovedCustomer)];

                    worker.RemoveTask(bestMovedCustomer, bestDistanceDeltaRemove);

                    bestInsertPartner.InsertTask(bestMovedCustomer, bestInsertIndex, bestDistanceDeltaInsert);

                    madeChange = true;
                }
            }
            while (madeChange);
        }

        public void InsertMultipleOpt(int neighborhoodRange, int numberOfInserts)
        {
            if (_team.Count < 2)
            {
                return;
            }

            bool madeChange;

            do
            {
                madeChange = false;

                bool foundCandidate = false;
                List<Location> bestMovedCustomers = null;
                Assembler bestInsertPartner = null;
                Double bestDistanceDeltaInsert = 0;
                Double bestDistanceDeltaRemove = 0;
                Double bestFitnessDeltaInsert = 0;
                Double bestFitnessDeltaRemove = 0;
                Int32 bestInsertIndex = -1;

                foreach (Location movedCustomer in _task.Customers)
                {
                    Assembler worker = ProcessorOf(movedCustomer);
                    List<Location> movedCustomerList = new List<Location>();
                    movedCustomerList.Add(movedCustomer);

                    while (movedCustomerList.Count < numberOfInserts && !worker.CustomerAfter(movedCustomerList.Last()).Equals(Location.HQ))
                    {
                        movedCustomerList.Add(worker.CustomerAfter(movedCustomerList.Last()));
                    }

                    List<Assembler> swapPartners = _team.ToList();

                    List<DistanceMatrixEntry> insertCustomerDistances = _task.DistanceMatrix[_task.Customers.IndexOf(movedCustomer)];
                        
                    foreach (Assembler swapPartner in swapPartners)
                    {
                        Int32 InsertIndex = -1;

                        Double newRawDistanceDeltaRemove = DistanceDelta(worker, movedCustomerList.ToArray());
                        Double newDistanceDeltaRemove = newRawDistanceDeltaRemove * _overallDistanceWeight;
                        Double newWorkloadDeltaRemove = (newDistanceDeltaRemove / 50) + (Math.Max((worker.Workload + (newDistanceDeltaRemove / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(worker.Workload - 8, 0) * _overtimePenaltyWeight);
                        Double newFitnessDeltaRemove = newDistanceDeltaRemove + newWorkloadDeltaRemove;

                        Double newRawDistanceDeltaInsert = DistanceDelta(swapPartner, movedCustomerList, insertCustomerDistances, out InsertIndex);
                        Double newDistanceDeltaInsert = newRawDistanceDeltaInsert * _overallDistanceWeight;
                        Double newWorkloadDeltaInsert = (newDistanceDeltaInsert / 50) + (Math.Max((swapPartner.Workload + (newDistanceDeltaInsert / 50)) - 8, 0) * _overtimePenaltyWeight - Math.Max(swapPartner.Workload - 8, 0) * _overtimePenaltyWeight);
                        Double newFitnessDeltaInsert = newDistanceDeltaInsert + newWorkloadDeltaInsert;

                        if (newFitnessDeltaRemove + newFitnessDeltaInsert < bestFitnessDeltaInsert + bestFitnessDeltaRemove && !swapPartner.Equals(worker))
                        {
                            bestDistanceDeltaRemove = newRawDistanceDeltaRemove;
                            bestDistanceDeltaInsert = newRawDistanceDeltaInsert;
                            bestFitnessDeltaRemove = newFitnessDeltaRemove;
                            bestFitnessDeltaInsert = newFitnessDeltaInsert;
                            bestMovedCustomers = movedCustomerList;
                            bestInsertPartner = swapPartner;
                            bestInsertIndex = InsertIndex;
                            foundCandidate = true;
                        }
                    }
                }

                if (foundCandidate)
                {
                    Assembler worker = ProcessorOf(bestMovedCustomers.First());

                    worker.RemoveTasks(bestMovedCustomers, bestDistanceDeltaRemove);

                    bestInsertPartner.InsertTasks(bestMovedCustomers, bestInsertIndex, bestDistanceDeltaInsert);
                    madeChange = true;
                }
            }
            while (madeChange);
        }

        /// <summary>
        /// Returns the nth next neighboring customer of all other subroutes
        /// </summary>
        /// <param name="customer">The customers who's next neighbors are looked up.</param>
        /// <param name="offset">The offset how close the neighbor should be in the ranking.</param>
        /// <returns>The neighbor with an offset in the ranking.</returns>
        private Location FindGlobalNeighbor(Location customer, Int32 offset)
        {
            Assembler worker = ProcessorOf(customer);
            List<DistanceMatrixEntry> distances = _task.Distances(customer).Where(dist => ProcessorOf(dist.Customer) != worker).ToList();

            if (offset < distances.Count)
            {
                return distances[offset].Customer;
            }

            if (distances.Count == 0)
            {
                return null;
            }

            return distances.Last().Customer;
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
            List<DistanceMatrixEntry> distances = _task.Distances(customer).Where(dist => subroutes.Contains(ProcessorOf(dist.Customer))).ToList();

            return (from distanceEntry in distances select distanceEntry.Customer).Take(maxOffset).ToList();
        }
        
        public Double DistanceDelta(Location piOfI, Location piOfIPlusOne, Location piOfJ, Location piOfJPlusOne)
        {
            Double fitnessDelta = 0;

            fitnessDelta -= _task.DistanceBetween(piOfI, piOfIPlusOne) + _task.DistanceBetween(piOfJ, piOfJPlusOne);
            fitnessDelta += _task.DistanceBetween(piOfI, piOfJ) + _task.DistanceBetween(piOfIPlusOne, piOfJPlusOne);

            return fitnessDelta;
        }

        /// <summary>
        /// Calculates the resulting distance delta if a swap of insertedCustomer with removedCustomer would occur. The Insert position will be 
        /// calculated by taking the neighbor of the inserted customer into account and check for two options, before or after that neighbor.
        /// </summary>
        /// <param name="worker">The assembler which is processing the removed customer and is supposed to take over insertedCustomer</param>
        /// <param name="insertedCustomer">The customer which will be added.</param>
        /// <param name="insertedDistanceMatrix">The List of distances correlating towards the inserted customer.</param>
        /// <param name="removedCustomer">The customer which will be removed.</param>
        /// <param name="insertedIndex">The out parameters determining the insert position used to calculate the delta.</param>
        /// <returns>The delta of the raw distance value.</returns>
        public Double DistanceDelta(Assembler worker, Location insertedCustomer, List<DistanceMatrixEntry> insertedDistanceMatrix, Location removedCustomer, out Int32 insertedIndex)
        {
            Double distanceDelta = 0;

            Location predecessor = worker.CustomerBefore(removedCustomer);
            Location successor = worker.CustomerAfter(removedCustomer);

            distanceDelta -= _task.DistanceBetween(predecessor, removedCustomer) + _task.DistanceBetween(removedCustomer, successor);
            distanceDelta += _task.DistanceBetween(predecessor, successor);

            foreach (DistanceMatrixEntry neighbor in insertedDistanceMatrix)
            {
                if (worker.Processes(neighbor.Customer) && !neighbor.Customer.Equals(removedCustomer))
                {
                    predecessor = neighbor.Customer;
                    break;
                }
            }

            Location prepredecessor = worker.CustomerBefore(predecessor);

            if (prepredecessor.Equals(removedCustomer))
            {
                prepredecessor = worker.CustomerBefore(prepredecessor);
            }

            successor = worker.CustomerAfter(predecessor);

            if (successor.Equals(removedCustomer))
            {
                successor = worker.CustomerAfter(successor);
            }

            Double beforePredecessorDelta = prepredecessor.DistanceTo(insertedCustomer) + insertedCustomer.DistanceTo(predecessor) - prepredecessor.DistanceTo(predecessor);
            Double afterPredecessorDelta = predecessor.DistanceTo(insertedCustomer) + insertedCustomer.DistanceTo(successor) - predecessor.DistanceTo(successor);

            if (beforePredecessorDelta < afterPredecessorDelta)
            {
                successor = predecessor;
                predecessor = prepredecessor;
            }

            distanceDelta -= _task.DistanceBetween(predecessor, successor);
            distanceDelta += _task.DistanceBetween(predecessor, insertedCustomer) + _task.DistanceBetween(insertedCustomer, successor);

            insertedIndex = worker.PositionOf(predecessor) + 1;

            if (worker.PositionOf(removedCustomer) < insertedIndex)
            {
                insertedIndex--;
            }

            return distanceDelta;
        }

        /// <summary>
        /// Calculates the resulting distance delta if a insert of insertedCustomer in worker would occur. The Insert position will be 
        /// calculated by taking the neighbor of the inserted customer into account and check for two options, before or after that neighbor.
        /// </summary>
        /// <param name="worker">The assembler which is processing the removed customer and is supposed to take over insertedCustomer</param>
        /// <param name="insertedCustomer">The customer which will be added.</param>
        /// <param name="insertedDistanceMatrix">The List of distances correlating towards the inserted customer.</param>
        /// <param name="insertedIndex">The out parameters determining the insert position used to calculate the delta.</param>
        /// <returns>The delta of the raw distance value.</returns>
        public Double DistanceDelta(Assembler worker, Location insertedCustomer, List<DistanceMatrixEntry> insertedDistanceMatrix, out Int32 insertedIndex)
        {
            Double distanceDelta = 0;

            Location predecessor = null;
            Location successor = null;

            foreach (DistanceMatrixEntry neighbor in insertedDistanceMatrix)
            {
                if (worker.Processes(neighbor.Customer))
                {
                    predecessor = neighbor.Customer;
                    break;
                }
            }

            Location prepredecessor = worker.CustomerBefore(predecessor);

            successor = worker.CustomerAfter(predecessor);

            Double beforePredecessorDelta = _task.DistanceBetween(prepredecessor, insertedCustomer) + _task.DistanceBetween(insertedCustomer, predecessor) - _task.DistanceBetween(prepredecessor, predecessor);
            Double afterPredecessorDelta = _task.DistanceBetween(predecessor, insertedCustomer) + _task.DistanceBetween(insertedCustomer, successor) - _task.DistanceBetween(predecessor, successor);

            if (beforePredecessorDelta < afterPredecessorDelta)
            {
                successor = predecessor;
                predecessor = prepredecessor;
            }

            distanceDelta -= _task.DistanceBetween(predecessor, successor);
            distanceDelta += _task.DistanceBetween(predecessor, insertedCustomer) + _task.DistanceBetween(insertedCustomer, successor);

            insertedIndex = worker.PositionOf(predecessor) + 1;

            return distanceDelta;
        }

        /// <summary>
        /// Calculates the resulting distance delta if an insert of all members of the insertedCustomers in worker would occur. The Insert position will be 
        /// calculated by taking the neighbor of the inserted customer into account and check for two options, before or after that neighbor.
        /// </summary>
        /// <param name="worker">The assembler which is processing the removed customer and is supposed to take over insertedCustomer</param>
        /// <param name="insertedCustomers">The customers which will be added.</param>
        /// <param name="insertedDistanceMatrix">The List of distances correlating towards the first inserted customer.</param>
        /// <param name="insertedIndex">The out parameters determining the insert position used to calculate the delta.</param>
        /// <returns>The delta of the raw distance value.</returns>
        public Double DistanceDelta(Assembler worker, List<Location> insertedCustomers, List<DistanceMatrixEntry> firstInsertedDistanceMatrix, out Int32 insertedIndex)
        {
            Double distanceDelta = 0;

            Location predecessor = null;
            Location successor = null;

            foreach (DistanceMatrixEntry neighbor in firstInsertedDistanceMatrix)
            {
                if (worker.Processes(neighbor.Customer))
                {
                    predecessor = neighbor.Customer;
                    break;
                }
            }

            Location prepredecessor = worker.CustomerBefore(predecessor);

            successor = worker.CustomerAfter(predecessor);

            Double beforePredecessorDelta = _task.DistanceBetween(prepredecessor, insertedCustomers.First()) + _task.DistanceBetween(insertedCustomers.Last(), predecessor) - _task.DistanceBetween(prepredecessor, predecessor);
            Double afterPredecessorDelta = _task.DistanceBetween(predecessor, insertedCustomers.First()) + _task.DistanceBetween(insertedCustomers.Last(), successor) - _task.DistanceBetween(predecessor, successor);

            if (beforePredecessorDelta < afterPredecessorDelta)
            {
                successor = predecessor;
                predecessor = prepredecessor;
            }

            distanceDelta -= _task.DistanceBetween(predecessor, successor);
            distanceDelta += _task.DistanceBetween(predecessor, insertedCustomers.First());

            foreach (Location customer in insertedCustomers)
            {
                if (customer.Equals(insertedCustomers.Last()))
                {
                    break;
                }

                distanceDelta += _task.DistanceBetween(customer, insertedCustomers[insertedCustomers.IndexOf(customer) + 1]);
            }

            distanceDelta += _task.DistanceBetween(insertedCustomers.Last(), successor);

            insertedIndex = worker.PositionOf(predecessor) + 1;

            return distanceDelta;
        }

        /// <summary>
        /// Calculates the resulting distance delta if a remove all removedCustomers would occur.
        /// </summary>
        /// <param name="worker">The assembler which is processing the removed customer and is supposed to take over insertedCustomer</param>
        /// <param name="removedCustomers">The array of customer which will be removed.</param>
        /// <returns>The delta of the raw distance value.</returns>
        public Double DistanceDelta(Assembler worker, params Location[] removedCustomers)
        {
            Double distanceDelta = 0;

            Location predecessor = worker.CustomerBefore(removedCustomers.First());
            Location successor = worker.CustomerAfter(removedCustomers.Last());

            distanceDelta -= _task.DistanceBetween(predecessor, removedCustomers.First());

            foreach (Location customer in removedCustomers)
            {
                if (customer.Equals(removedCustomers.Last()))
                {
                    break;
                }

                distanceDelta -= _task.DistanceBetween(customer, removedCustomers[removedCustomers.ToList().IndexOf(customer) + 1]);
            }

            distanceDelta -= _task.DistanceBetween(removedCustomers.Last(), successor);

            distanceDelta += _task.DistanceBetween(predecessor, successor);

            return distanceDelta;
        }


        private void AssignTask(Location customer, Assembler worker, List<Location> unassignedCustomers)
        {
            Double distanceDelta = 0;

            if (worker.CustomersAssigned.Count > 0)
            {
                distanceDelta -= _task.DistanceBetween(worker.LastCustomer, Location.HQ);
                distanceDelta += _task.DistanceBetween(worker.LastCustomer, customer) + _task.DistanceBetween(customer, Location.HQ);
            }
            else
            {
                distanceDelta += _task.DistanceBetween(Location.HQ, customer) + _task.DistanceBetween(customer, Location.HQ);
            }

            worker.AcceptTask(customer, distanceDelta);
            unassignedCustomers.Remove(customer);
        }

        private Assembler ProcessorOf(Location customer)
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
