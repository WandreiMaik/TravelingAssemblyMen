using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelingAssemblyMen.Model
{
    public class TAP
    {
        private List<Location> _customerList;
        private Solution _solution;
        private List<List<DistanceMatrixEntry>> _distanceMatrix;

        public Int32 NumberOfCustomers
        {
            get
            {
                return _customerList.Count;
            }
        }

        public Int32 NumberOfAssembler
        {
            get
            {
                 return _solution.NumberOfAssembler;
            }
        }
        
        public List<Location> Customers
        {
            get
            {
                return _customerList;
            }
        }

        public List<List<DistanceMatrixEntry>> DistanceMatrix
        {
            get
            {
                return _distanceMatrix;
            }
        }

        public Double OvertimePenaltyWeight
        {
            set { _solution.OvertimePenaltyWeigth = value; }
        }

        public Double OverallWorkloadWeight
        {
            set { _solution.OverallWorkloadWeight = value; }
        }

        #region Constructors
        public TAP(Int32 numberOfAssemblers, Int32 numberOfCustomers, Double overtimePenaltyWeight, Double overallDistanceWeight)
        {
            _customerList = new List<Location>();

            while (_customerList.Count < numberOfCustomers)
            {
                Location newCustomer = new Location((MyRNG.NextDouble() * 100) - 50, (MyRNG.NextDouble() * 100) - 50, _customerList.Count.ToString());

                if (_customerList.Contains(newCustomer))
                {
                    continue;
                }

                _customerList.Add(newCustomer);
            }

            CalculateDistanceMatrix();

            _solution = new Solution(this, numberOfAssemblers, overtimePenaltyWeight, overallDistanceWeight);
        }

        public TAP(Int32 numberOfAssemblers, List<Location> customers, Double overtimePenaltyWeight, Double overallDistanceWeight)
        {
            _customerList = customers ?? new List<Location>();
            CalculateDistanceMatrix();
            _solution = new Solution(this, numberOfAssemblers, overtimePenaltyWeight, overallDistanceWeight);
        } 
        #endregion

        public void SolveRandomly()
        {
            _solution.SolveRandomly();
        }

        public void SolveGreedy()
        {
            _solution.SolveGreedy();
        }

        public void DrawSolution(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            ColorPicker.Reset();
            _solution.DrawSolution(graphics, origin, pixelsPerKilometer);
        }

        public string FitnessValue()
        {
            return Math.Round(_solution.FitnessValue(), 10).ToString("F10");
        }
        
        private void CalculateDistanceMatrix()
        {
            _distanceMatrix = new List<List<DistanceMatrixEntry>>();

            foreach (Location customer in _customerList)
            {
                List<DistanceMatrixEntry> distances = new List<DistanceMatrixEntry>();

                distances.Add(new DistanceMatrixEntry(Location.HQ, customer));

                foreach (Location comparate in _customerList)
                {
                    if (comparate.Equals(customer))
                    {
                        continue;
                    }

                    distances.Add(new DistanceMatrixEntry(comparate, customer));
                }

                _distanceMatrix.Add(distances.OrderBy(entry => entry.Distance).ToList());
            }

            // distances from HQ to all customers are inside the last entry
            List<DistanceMatrixEntry> hqDistances = new List<DistanceMatrixEntry>();

            foreach (Location comparate in _customerList)
            {
                hqDistances.Add(new DistanceMatrixEntry(comparate, Location.HQ));
            }

            _distanceMatrix.Add(hqDistances.OrderBy(entry => entry.Distance).ToList());
        }

        public Double DistanceBetween(Location one, Location another)
        {
            Int32 oneIndex = _customerList.IndexOf(one);

            if (one.Equals(Location.HQ))
            {
                oneIndex = _customerList.Count;
            }

            if (oneIndex == -1)
            {
                return one.DistanceTo(another);
            }

            List<DistanceMatrixEntry> test = _distanceMatrix[oneIndex].Where(dist => dist.Customer.Equals(another)).ToList();

            return test.First().Distance;
        }

        public List<DistanceMatrixEntry> Distances(Location customer)
        {
            Int32 customerIndex = _customerList.IndexOf(customer);

            if (customer.Equals(Location.HQ))
            {
                customerIndex = _customerList.Count;
            }

            return _distanceMatrix[customerIndex];
        }

        #region IO
        public void Save(string fileName)
        {
            using (StreamWriter fileWriter = new StreamWriter(fileName))
            {
                fileWriter.WriteLine("TravelingAssemblymenProblem - Version 1.0");
                fileWriter.WriteLine();
                fileWriter.WriteLine("Number of Assembler: " + _solution.NumberOfAssembler);
                fileWriter.WriteLine("Number of Customers: " + _customerList.Count);
                fileWriter.WriteLine();

                foreach (Location customer in _customerList)
                {
                    fileWriter.WriteLine(customer.ToString());
                }
            }
        }

        public static TAP Open(string fileName, Double overtimePenaltyWeight, Double overallDistanceWeight)
        {
            using (StreamReader fileReader = new StreamReader(fileName))
            {
                string headline = fileReader.ReadLine();
                fileReader.ReadLine();
                string assembercountString = fileReader.ReadLine();
                string customercountString = fileReader.ReadLine();
                fileReader.ReadLine();

                int numberOfAssemblers = -1;
                int numberOfCustomers = -1;

                if (assembercountString.Contains("Number of Assembler: "))
                {
                    numberOfAssemblers = Int32.Parse(assembercountString.Substring(assembercountString.IndexOf(":") + 2));
                }

                if (numberOfAssemblers == -1)
                {
                    throw new Exception();
                }

                if (customercountString.Contains("Number of Customers: "))
                {
                    numberOfCustomers = Int32.Parse(customercountString.Substring(customercountString.IndexOf(":") + 2));
                }

                if (numberOfCustomers == -1)
                {
                    throw new Exception();
                }

                List<Location> customers = new List<Location>();

                while (customers.Count < numberOfCustomers)
                {
                    customers.Add(new Location(fileReader.ReadLine(), customers.Count.ToString()));
                }

                return new TAP(numberOfAssemblers, customers, overtimePenaltyWeight, overallDistanceWeight);
            }
        }

        internal void LocalOptimisation(LocalOptimisationStyle style, int neighborhoodRange)
        {
            if (style == LocalOptimisationStyle.TwoOpt)
            {
                _solution.TwoOpt(neighborhoodRange);
            }

            if (style == LocalOptimisationStyle.Swap)
            {
                _solution.SwapOpt(neighborhoodRange);
            }

            if (style == LocalOptimisationStyle.Insert)
            {
                //_solution.InsertOpt(neighborhoodRange);
            }
        }
        #endregion
    }
}
