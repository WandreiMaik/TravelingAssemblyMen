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
        private Double[,] _distanceMatrix;

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

        #region Constructors
        public TAP(Int32 numberOfAssemblers, Int32 numberOfCustomers)
        {
            _customerList = new List<Location>();

            while (_customerList.Count < numberOfCustomers)
            {
                Location newCustomer = new Location((MyRNG.NextDouble() * 100) - 50, (MyRNG.NextDouble() * 100) - 50);

                if (_customerList.Contains(newCustomer))
                {
                    continue;
                }

                _customerList.Add(newCustomer);
            }

            CalculateDistanceMatrix();

            _solution = new Solution(this, numberOfAssemblers);
        }

        public TAP(Int32 numberOfAssemblers, List<Location> customers)
        {
            _customerList = customers ?? new List<Location>();
            CalculateDistanceMatrix();
            _solution = new Solution(this, numberOfAssemblers);
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

        public string FitnessValue(Double overtimePenaltyWeight, Double overallWorkloadWeight)
        {
            return Math.Round(_solution.FitnessValue(overtimePenaltyWeight, overallWorkloadWeight), 10).ToString("F10");
        }
        
        private void CalculateDistanceMatrix()
        {
            _distanceMatrix = new Double[_customerList.Count, _customerList.Count];

            for (int horizontalIndex = 0; horizontalIndex < _customerList.Count; horizontalIndex++)
            {
                for (int verticalIndex = 0; verticalIndex < _customerList.Count; verticalIndex++)
                {
                    _distanceMatrix[horizontalIndex, verticalIndex] = _customerList[horizontalIndex].DistanceTo(_customerList[verticalIndex]);
                }
            }
        }

        public Double DistanceBetween(Location one, Location another)
        {
            Int32 oneIndex = _customerList.IndexOf(one);
            Int32 anotherIndex = _customerList.IndexOf(another);

            if (oneIndex < 0 || anotherIndex < 0)
            {
                return one.DistanceTo(another);
            }

            return _distanceMatrix[oneIndex, anotherIndex];
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

        public static TAP Open(string fileName)
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
                    customers.Add(new Location(fileReader.ReadLine()));
                }

                return new TAP(numberOfAssemblers, customers);
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
