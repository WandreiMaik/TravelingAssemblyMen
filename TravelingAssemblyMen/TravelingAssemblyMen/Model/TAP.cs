using System;
using System.Collections.Generic;
using System.Drawing;
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
    }
}
