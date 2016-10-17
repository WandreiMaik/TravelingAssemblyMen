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
        private List<Assembler> _assemblerList;

        public Solution(List<Location> customerList, int assemblerCount)
        {
            _taskDistribution = new Dictionary<Location, Int32>();
            _customerList = customerList.ToList<Location>();
            _assemblerList = new List<Assembler>();

            while (_assemblerList.Count < assemblerCount)
            {
                _assemblerList.Add(new Assembler());
            }
        }

        public void SolveRandomly()
        {
            _taskDistribution.Clear();
            
            foreach (Assembler worker in _assemblerList)
            {
                worker.RemoveEveryTask();
            }

            foreach (Location customer in _customerList)
            {
                Int32 assembler = MyRNG.Next(_assemblerList.Count);

                _taskDistribution.Add(customer, assembler);
                _assemblerList[assembler].AcceptTask(customer, CustomerInsertStyle.Random);
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
            Dictionary<Location, Int32>.KeyCollection processedTasks = _taskDistribution.Keys;
            List<Location> forgottenCustomers = _customerList.Except(processedTasks).ToList();

            if (forgottenCustomers.Count > 0)
            {
                return Double.MaxValue;
            }

            return fitness;
        }

        internal void DrawSolution(Graphics graphics, Position origin, Double pixelsPerKilometer)
        {
            foreach (Location customer in _customerList)
            {
                customer.Draw(graphics, origin, pixelsPerKilometer);
            }

            foreach (Assembler worker in _assemblerList)
            {
                worker.Draw(graphics, origin, pixelsPerKilometer);
            }
        }
    }
}
