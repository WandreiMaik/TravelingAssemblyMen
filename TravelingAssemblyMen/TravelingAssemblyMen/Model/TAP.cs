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

        public Int32 NumberOfCustomers
        {
            get
            {
                if (_customerList != null)
                {
                    return _customerList.Count;
                }

                return 1;
            }
        }

        public Int32 NumberOfAssembler
        {
            get
            {
                if (_solution != null)
                {
                    return _solution.NumberOfAssembler;
                }

                return 1;
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

            _solution = new Solution(_customerList, numberOfAssemblers);
        }

        public TAP(Int32 numberOfAssemblers, List<Location> customers)
        {
            _customerList = customers;
            _solution = new Solution(_customerList, numberOfAssemblers);
        } 
        #endregion

        public void SolveRandomly()
        {
            _solution.SolveRandomly();
        }

        public void PaintPanel(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawEllipse(new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.Red)), new System.Drawing.Rectangle(20, 20, 200, 200));
        }

        public void DrawSolution(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {

            _solution.DrawSolution(graphics, origin, pixelsPerKilometer);
        }

        public string FitnessValue(Double overtimePenaltyWeight, Double overallWorkloadWeight)
        {
            return Math.Round(_solution.FitnessValue(overtimePenaltyWeight, overallWorkloadWeight), 10).ToString("F10");
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
        #endregion
    }
}
